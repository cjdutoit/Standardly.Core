// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Configurations.Statuses;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Orchestrations.Templates;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationService : ITemplateOrchestrationService
    {
        public event Action<DateTimeOffset, string, string> LogRaised = delegate { };
        public bool ScriptExecutionIsEnabled { get; set; } = true;
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

        public List<Template> FindAllTemplates() =>
            TryCatch(() =>
            {
                List<Template> templates = new List<Template>();

                var fileList = this.fileProcessingService
                    .RetrieveListOfFiles(
                    this.templateConfig.TemplateFolderPath,
                    this.templateConfig.TemplateDefinitionFileName);

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = this.fileProcessingService.ReadFromFile(file);

                        Template template = this.templateProcessingService
                            .ConvertStringToTemplate(rawTemplate);

                        templates.Add(template);
                    }
                    catch (Exception)
                    {
                    }
                }

                return templates;
            });

        public void GenerateCode(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary) =>
            TryCatch(() =>
                {
                    this.LogMessage(DateTimeOffset.UtcNow, $"Validating inputs");
                    ValidateTemplateArguments(templates, replacementDictionary);
                    List<Template> templatesToGenerate = new List<Template>();
                    this.LogMessage(DateTimeOffset.UtcNow, $"Check what needs doing on the templates");

                    if (!replacementDictionary.ContainsKey("$currentBranch$"))
                    {
                        replacementDictionary.Add("$currentBranch$", replacementDictionary["$basebranch$"]);
                    }

                    if (!replacementDictionary.ContainsKey("$previousBranch$"))
                    {
                        replacementDictionary.Add("$previousBranch$", replacementDictionary["$basebranch$"]);
                    }

                    templatesToGenerate.AddRange(
                        GetOnlyTheTemplatesThatRequireGeneratingCode(templates, replacementDictionary));

                    this.previousBranch =
                        !string.IsNullOrWhiteSpace(replacementDictionary["$previousBranch$"])
                            ? replacementDictionary["$previousBranch$"]
                            : replacementDictionary["$basebranch$"];

                    templatesToGenerate.ForEach(template =>
                    {
                        this.LogMessage(DateTimeOffset.UtcNow, $"Generating templates");
                        GenerateTemplate(template, replacementDictionary);
                    });
                });

        private void GenerateTemplate(
            Template template,
            Dictionary<string, string> replacementDictionary)
        {
            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Starting code generation for {template.Name}");

            for (int taskIndex = 0; taskIndex <= template.Tasks.Count - 1; taskIndex++)
            {
                var currentBranchName =
                    this.templateProcessingService
                        .TransformString(template.Tasks[taskIndex].BranchName, replacementDictionary);

                replacementDictionary["$currentBranch$"] = currentBranchName;

                var transformedTemplate =
                    this.templateProcessingService
                        .TransformTemplate(template, replacementDictionary);

                PerformTasks(
                    transformedTemplate.Tasks[taskIndex],
                    transformedTemplate, replacementDictionary);

                this.previousBranch = this.templateProcessingService
                        .TransformString(transformedTemplate.Tasks[taskIndex].BranchName, replacementDictionary);
            }
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
            task.Actions.ForEach(action =>
            {
                this.LogMessage(
                    DateTimeOffset.UtcNow,
                    $"Starting with {transformedTemplate.Name} > {task.Name} > {action.Name}");

                this.PerformFileCreations(action.Files, replacementDictionary);
                this.PerformAppendOpperations(action.Appends);
                this.PerformExecutions(action.Executions, action.ExecutionFolder);
            });
        }

        private void PerformFileCreations(
            List<Models.Foundations.Templates.Tasks.Actions.Files.File> files,
            Dictionary<string, string> replacementDictionary)
        {
            files.ForEach(file =>
            {
                string sourceString = this.fileProcessingService.ReadFromFile(file.Template);

                string transformedSourceString =
                    this.templateProcessingService.TransformString(sourceString, replacementDictionary);

                transformedSourceString = transformedSourceString.Replace("##n##", "\\n");

                var fileExists = this.fileProcessingService.CheckIfFileExists(file.Target);
                var isRequired = !fileExists || file.Replace == true;

                if (isRequired)
                {
                    this.LogMessage(
                        DateTimeOffset.UtcNow,
                        $"Adding file '{file.Target}'");

                    this.fileProcessingService.WriteToFile(file.Target, transformedSourceString);
                }
            });
        }

        private void PerformAppendOpperations(List<Append> appends)
        {
            foreach (Append append in appends)
            {
                string fileContent = this.fileProcessingService.ReadFromFile(append.Target);

                string appendedContent = this.templateProcessingService.AppendContent(
                    sourceContent: fileContent,
                    doesNotContainContent: append.DoesNotContainContent,
                    regexToMatchForAppend: append.RegexToMatchForAppend,
                    appendContent: append.ContentToAppend,
                    appendToBeginning: append.AppendToBeginning,
                    appendEvenIfContentAlreadyExist: append.AppendEvenIfContentAlreadyExist);

                this.fileProcessingService.WriteToFile(append.Target, appendedContent);
            }
        }

        private void PerformExecutions(
            List<Models.Foundations.Executions.Execution> executions,
            string executionFolder)
        {
            if (this.ScriptExecutionIsEnabled == true)
            {
                string outcome = this.executionProcessingService.Run(executions, executionFolder);
                this.LogMessage(DateTimeOffset.UtcNow, $"{outcome}");
            }
            else
            {
                StringBuilder scripts = new StringBuilder();
                scripts.AppendLine("Skipping the following executions as executions currently disabled:");

                foreach (var execution in executions)
                {
                    scripts.AppendLine(execution.Instruction);
                }

                this.LogMessage(DateTimeOffset.UtcNow, scripts.ToString());
            }
        }

        private void LogMessage(DateTimeOffset date, string message)
        {
            this.LogRaised(date, $"", Status.Information);
            this.loggingBroker.LogInformation($"{date} - {message}");
        }

        private List<Template> GetOnlyTheTemplatesThatRequireGeneratingCode(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            List<Template> requiredTemplates = new List<Template>();

            foreach (Template template in templates)
            {
                var transformedTemplate =
                    this.templateProcessingService
                        .TransformTemplate(template, replacementDictionary);

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
            var actionsToRemove = RemoveActionIfNotRequired(actionsWithFiles);

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

        private List<Models.Foundations.Templates.Tasks.Actions.Action> RemoveActionIfNotRequired(
            List<Models.Foundations.Templates.Tasks.Actions.Action> actions)
        {
            List<Models.Foundations.Templates.Tasks.Actions.Action> actionsToRemove =
                new List<Models.Foundations.Templates.Tasks.Actions.Action>();

            actions.ForEach(action =>
            {
                foreach (Models.Foundations.Templates.Tasks.Actions.Files.File file in action.Files.ToList())
                {
                    bool fileExist = this.fileProcessingService.CheckIfFileExists(file.Target);
                    bool isRequired = fileExist == false || (fileExist && file.Replace == true);

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
