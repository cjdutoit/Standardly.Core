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
            this.loggingBroker = loggingBroker;
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

        public ValueTask GenerateCodeAsync(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary) =>
            TryCatchAsync(async () =>
                {
                    ValidateTemplateArguments(templates, replacementDictionary);

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
                });

        private async ValueTask GenerateTemplateAsync(
            Template template,
            Dictionary<string, string> replacementDictionary)
        {
            var transformedTemplate =
                await this.templateProcessingService
                    .TransformTemplateAsync(template, replacementDictionary);

            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Starting code generation for {transformedTemplate.Name}");

            transformedTemplate.Tasks.ForEach(task =>
            {
                PerformTasks(task, transformedTemplate, replacementDictionary);
            });
        }

        private void PerformTasks(
            Models.Foundations.Templates.Tasks.Task task,
            Template transformedTemplate,
            Dictionary<string, string> replacementDictionary)
        {
            previousBranch = task.BranchName;

            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Starting with {transformedTemplate.Name} > {task.Name}");

            PerformActions(task, transformedTemplate, replacementDictionary);
        }

        private void PerformActions(
            Models.Foundations.Templates.Tasks.Task task,
            Template transformedTemplate,
            Dictionary<string, string> replacementDictionary)
        {
            task.Actions.ForEach(async action =>
            {
                this.LogMessage(
                    DateTimeOffset.UtcNow,
                    $"Starting with {transformedTemplate.Name} > {task.Name} > {action.Name}");

                this.PerformFileCreations(action.Files, replacementDictionary);
                this.PerformAppendOpperations(action.Appends);
                await this.PerformExecutionsAsync(action.Executions, action.ExecutionFolder);
            });
        }

        private void PerformFileCreations(
            List<Models.Foundations.Templates.Tasks.Actions.Files.File> files,
            Dictionary<string, string> replacementDictionary)
        {
            files.ForEach(async file =>
            {
                string sourceString = await this.fileProcessingService.ReadFromFileAsync(file.Template);

                string transformedSourceString =
                    await this.templateProcessingService.TransformStringAsync(sourceString, replacementDictionary);

                transformedSourceString = transformedSourceString.Replace("##n##", "\\n");

                var fileExists = this.fileProcessingService.CheckIfFileExistsAsync(file.Target).Result;
                var isRequired = !fileExists || file.Replace == true;

                if (isRequired)
                {
                    this.LogMessage(
                        DateTimeOffset.UtcNow,
                        $"Adding file '{file.Target}'");

                    await this.fileProcessingService.WriteToFileAsync(file.Target, transformedSourceString);
                }
            });
        }

        private void PerformAppendOpperations(List<Models.Foundations.Templates.Tasks.Actions.Appends.Append> appends)
        {
            appends.ForEach(async append =>
            {
                await this.templateProcessingService.AppendContentAsync(
                    sourceContent: append.Target,
                    regexToMatchForAppend: append.RegexToMatchForAppend,
                    appendContent: append.ContentToAppend,
                    appendToBeginning: append.AppendToBeginning,
                    appendEvenIfContentAlreadyExist: append.AppendEvenIfContentAlreadyExist
                    );
            });
        }

        private async ValueTask PerformExecutionsAsync(
            List<Models.Foundations.Executions.Execution> executions,
            string executionFolder)
        {
            string outcome = await this.executionProcessingService.RunAsync(executions, executionFolder);
            this.LogMessage(DateTimeOffset.UtcNow, $"{outcome}");
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
                        .TransformTemplateAsync(template, replacementDictionary);

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

            tasks.ToList().ForEach(task =>
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

            actions.ForEach(action =>
            {
                foreach (Models.Foundations.Templates.Tasks.Actions.Files.File file in action.Files.ToList())
                {
                    bool isRequired =
                        this.fileProcessingService.CheckIfFileExistsAsync(file.Target).Result
                        && file.Replace == true;

                    if (!isRequired)
                    {
                        action.Files.Remove(file);
                    }
                }

                if (action.Files.Count == 0)
                {
                    actionsToRemove.Add(action);
                }
            });

            return actionsToRemove;
        }
    }
}
