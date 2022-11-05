// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Configurations.Statuses;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.Templates;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationService : ITemplateOrchestrationService
    {
        public event Action<DateTimeOffset, string, string> LogRaised = delegate { };
        private readonly IFileProcessingService fileProcessingService;
        private readonly IExecutionProcessingService executionProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;
        private readonly ITemplateConfig templateConfig;
        private readonly ILoggingBroker loggingBroker;
        private string previousBranch = string.Empty;

        public TemplateOrchestrationService(
            IFileProcessingService fileProcessingService,
            IExecutionProcessingService executionProcessingService,
            ITemplateProcessingService templateProcessingService,
            ITemplateConfig templateConfig,
            ILoggingBroker loggingBroker)
        {
            this.fileProcessingService = fileProcessingService;
            this.executionProcessingService = executionProcessingService;
            this.templateProcessingService = templateProcessingService;
            this.templateConfig = templateConfig;
        }

        public ValueTask<List<Template>> FindAllTemplatesAsync() =>
            TryCatchAsync(async () =>
            {
                List<Template> templates = new List<Template>();

                var fileList = await this.fileProcessingService
                    .RetrieveListOfFilesAsync(
                    this.templateConfig.TemplateFolder,
                    this.templateConfig.TemplateDefinitionFile);

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = await this.fileProcessingService.ReadFromFileAsync(file);

                        Template template = await this.templateProcessingService
                            .ConvertStringToTemplateAsync(rawTemplate);

                        templates.Add(template);
                    }
                    catch (Exception)
                    {
                    }
                }

                return templates;
            });

        public async ValueTask GenerateCodeAsync(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            this.previousBranch = !string.IsNullOrWhiteSpace(replacementDictionary["$previousBranch$"])
                ? replacementDictionary["$previousBranch$"]
                : replacementDictionary["$basebranch$"];

            List<Template> templatesToGenerate = new List<Template>();

            templatesToGenerate.AddRange(
                await GetOnlyTheTemplatesThatRequireGeneratingCodeAsync(templates, replacementDictionary));

            templatesToGenerate.ForEach(async template =>
            {
                replacementDictionary["$previousBranch$"] = previousBranch;
                await GenerateTemplateAsync(template, replacementDictionary);
            });
        }

        private async ValueTask GenerateTemplateAsync(
            Template template,
            Dictionary<string, string> replacementDictionary)
        {
            var transformedTemplate =
                await this.templateProcessingService
                    .TransformTemplateAsync(template, replacementDictionary, '$');

            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Staring code generation for {transformedTemplate.Name}");

            transformedTemplate.Tasks.ForEach(task =>
            {
                PerformTasks(task, transformedTemplate);
            });
        }

        private void PerformTasks(Models.Foundations.Templates.Tasks.Task task, Template transformedTemplate)
        {
            previousBranch = task.BranchName;

            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Staring with {transformedTemplate.Name} > {task.Name}");

            PerformActions(task, transformedTemplate);
        }

        private void PerformActions(Models.Foundations.Templates.Tasks.Task task, Template transformedTemplate)
        {
            task.Actions.ForEach(action =>
            {
                this.LogMessage(
                    DateTimeOffset.UtcNow,
                    $"Staring with {transformedTemplate.Name} > {task.Name} > {action.Name}");

                this.PerformFileCreations(action.Files);
                this.PerformAppendOpperations(action.Appends);
                this.PerformExecutions(action.Executions);
            });
        }

        private void PerformFileCreations(List<Models.Foundations.Templates.Tasks.Actions.Files.File> files)
        {
            files.ForEach(async file =>
            {

                string sourceString = await this.fileProcessingService.ReadFromFileAsync(file.Template);

                string transformedSourceString = await this.templateProcessingService..TransformString(sourceString, replacementDictionary)
                                                    .Replace("##n##", "\\n");

            });
        }

        private void PerformAppendOpperations(Models.Foundations.Templates.Tasks.Actions.Appends.Append appends)
        {
            appends.ForEach(append => { });
        }

        private void PerformExecutions(List<Models.Foundations.Executions.Execution> executions)
        {
            executions.ForEach(execution => { });
        }

        private void LogMessage(DateTimeOffset date, string message)
        {
            this.LogRaised(date, $"", Status.Information);
            this.loggingBroker.LogInformation($"{date} - {message}");
        }

        private async ValueTask<List<Template>> GetOnlyTheTemplatesThatRequireGeneratingCodeAsync(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            List<Template> requiredTemplates = new List<Template>();

            foreach (Template template in templates)
            {
                var transformedTemplate =
                    await this.templateProcessingService
                        .TransformTemplateAsync(template, replacementDictionary, '$');

                bool templateRequired = CheckIfTemplateIsRequired(transformedTemplate);

                if (templateRequired)
                {
                    requiredTemplates.Add(template);
                }
            }

            return requiredTemplates;
        }

        private bool CheckIfTemplateIsRequired(Template template)
        {
            var tasks = template.Tasks;

            tasks.ForEach(task =>
            {
                tasks.Remove(RemoveTaskIfNotrequired(task));
            });

            return template.Tasks.Any();
        }

        private Models.Foundations.Templates.Tasks.Task RemoveTaskIfNotrequired(
            Models.Foundations.Templates.Tasks.Task task)
        {
            var actions = task.Actions;
            var actionsWithFiles = actions.Where(action => action.Files.Count > 0).ToList();
            var actionsToRemove = RemoveActionIfNotRequiredAsync(actionsWithFiles);

            actionsToRemove.ForEach(action =>
            {
                actionsWithFiles.Remove(action);
            });

            if (actionsWithFiles.Count == 0)
            {
                return task;
            }

            return null;
        }

        private List<Models.Foundations.Templates.Tasks.Actions.Action> RemoveActionIfNotRequiredAsync(
            List<Models.Foundations.Templates.Tasks.Actions.Action> actions)
        {
            List<Models.Foundations.Templates.Tasks.Actions.Action> actionsToRemove =
                new List<Models.Foundations.Templates.Tasks.Actions.Action>();

            actions.ForEach(async action =>
            {
                List<Models.Foundations.Templates.Tasks.Actions.Files.File> files = action.Files;

                foreach (Models.Foundations.Templates.Tasks.Actions.Files.File file in action.Files)
                {
                    if (await this.fileProcessingService
                        .CheckIfFileExistsAsync(file.Target) && file.Replace == false)
                    {
                        files.Remove(file);
                    }
                }

                if (files.Count == 0)
                {
                    actionsToRemove.Add(action);
                }
            });

            return actionsToRemove;
        }
    }
}
