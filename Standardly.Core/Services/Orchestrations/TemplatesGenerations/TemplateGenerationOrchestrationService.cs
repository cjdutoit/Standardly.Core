// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Standardly.Core.Models.Configurations.Statuses;
using Standardly.Core.Models.Events;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Orchestrations;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplatesGenerations
{
    public partial class TemplateGenerationOrchestrationService : ITemplateGenerationOrchestrationService
    {
        public event EventHandler<ProcessedEventArgs> Processed;

        private readonly IFileProcessingService fileProcessingService;
        private readonly IExecutionProcessingService executionProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;
        //private string previousBranch = string.Empty;
        private int processedItems { get; set; }
        private int totalItems { get; set; }

        public TemplateGenerationOrchestrationService(
            IFileProcessingService fileProcessingService,
            IExecutionProcessingService executionProcessingService,
            ITemplateProcessingService templateProcessingService)
        {
            this.fileProcessingService = fileProcessingService;
            this.executionProcessingService = executionProcessingService;
            this.templateProcessingService = templateProcessingService;
        }

        public ValueTask GenerateCodeAsync(TemplateGenerationInfo templateGenerationInfo) =>
            TryCatch(async () =>
                {
                    this.LogMessage(DateTimeOffset.UtcNow, $"Validating inputs");
                    ValidateTemplateGenerationInfoIsNotNull(templateGenerationInfo);
                    ValidateTemplateArguments(templateGenerationInfo);
                    List<Template> templatesToGenerate = new List<Template>();
                    this.LogMessage(DateTimeOffset.UtcNow, $"Check what needs doing on the templates");

                    if (!templateGenerationInfo.ReplacementDictionary.ContainsKey("$currentBranch$"))
                    {
                        templateGenerationInfo.ReplacementDictionary
                            .Add("$currentBranch$", templateGenerationInfo.ReplacementDictionary["$basebranch$"]);
                    }

                    if (!templateGenerationInfo.ReplacementDictionary.ContainsKey("$previousBranch$"))
                    {
                        templateGenerationInfo.ReplacementDictionary
                            .Add("$previousBranch$", templateGenerationInfo.ReplacementDictionary["$basebranch$"]);
                    }

                    var templatesFound = await GetOnlyTheTemplatesThatRequireGeneratingCodeAsync(
                            templateGenerationInfo.Templates,
                            templateGenerationInfo.ReplacementDictionary);

                    templatesToGenerate.AddRange(templatesFound);

                    this.processedItems = 0;
                    this.totalItems = 0;

                    templatesToGenerate.ForEach(template =>
                    {
                        template.Tasks.ForEach(task =>
                        {
                            this.totalItems += task.Actions.Count();
                        });
                    });

                    foreach (var template in templatesToGenerate)
                    {
                        this.LogMessage(DateTimeOffset.UtcNow, $"Generating templates");

                        await GenerateTemplateAsync(
                            template,
                            templateGenerationInfo);
                    }
                });

        private async ValueTask GenerateTemplateAsync(
            Template template,
            TemplateGenerationInfo templateGenerationInfo)
        {
            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Starting code generation for {template.Name}");

            for (int taskIndex = 0; taskIndex <= template.Tasks.Count - 1; taskIndex++)
            {
                var currentBranchName =
                    await this.templateProcessingService
                        .TransformStringAsync(
                            content: template.Tasks[taskIndex].BranchName,
                            replacementDictionary: templateGenerationInfo.ReplacementDictionary);

                templateGenerationInfo.ReplacementDictionary["$currentBranch$"] = currentBranchName;
                template.ReplacementDictionary = templateGenerationInfo.ReplacementDictionary;

                var transformedTemplate =
                    await this.templateProcessingService
                        .TransformTemplateAsync(template);

                await PerformTaskAsync(
                    task: transformedTemplate.Tasks[taskIndex],
                    transformedTemplate,
                    templateGenerationInfo: templateGenerationInfo);

                var previousBranch =
                    await this.templateProcessingService
                        .TransformStringAsync(
                            content: transformedTemplate.Tasks[taskIndex].BranchName,
                            replacementDictionary: templateGenerationInfo.ReplacementDictionary);

                templateGenerationInfo.ReplacementDictionary["$previousBranch$"] = previousBranch;
                template.ReplacementDictionary = templateGenerationInfo.ReplacementDictionary;
            }

            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Completed code generation for required templates.");
        }

        private async ValueTask PerformTaskAsync(
            Models.Foundations.Templates.Tasks.Task task,
            Template transformedTemplate,
            TemplateGenerationInfo templateGenerationInfo)
        {
            this.LogMessage(
                DateTimeOffset.UtcNow,
                $"Starting with {transformedTemplate.Name} > {task.Name}");

            await PerformActionsAsync(
                task,
                transformedTemplate,
                templateGenerationInfo);
        }

        private async ValueTask PerformActionsAsync(
            Models.Foundations.Templates.Tasks.Task task,
            Template transformedTemplate,
            TemplateGenerationInfo templateGenerationInfo)
        {
            foreach (Models.Foundations.Templates.Tasks.Actions.Action action in task.Actions)
            {
                this.LogMessage(
                    DateTimeOffset.UtcNow,
                    $"Starting with {transformedTemplate.Name} > {task.Name} > {action.Name}");

                this.PerformFileCreations(action.Files, templateGenerationInfo);
                await this.PerformAppendOpperationsAsync(action.Appends, templateGenerationInfo);
                this.PerformExecutions(action.Executions, action.ExecutionFolder, templateGenerationInfo);

                this.processedItems += 1;
            }
        }

        private void PerformFileCreations(
            List<Models.Foundations.Templates.Tasks.Actions.Files.File> files,
            TemplateGenerationInfo templateGenerationInfo)
        {
            files.ForEach(file =>
            {
                string sourceString = this.fileProcessingService.ReadFromFileAsync(file.Template).Result;

                string transformedSourceString =
                    this.templateProcessingService.TransformStringAsync(
                        content: sourceString,
                        replacementDictionary: templateGenerationInfo.ReplacementDictionary).Result;

                transformedSourceString = transformedSourceString.Replace("##n##", "\\n");

                var fileExists = this.fileProcessingService.CheckIfFileExistsAsync(file.Target).Result;
                var isRequired = !fileExists || file.Replace == true;

                if (isRequired)
                {
                    this.LogMessage(
                        DateTimeOffset.UtcNow,
                        $"Adding file '{file.Target}'");

                    bool result = this.fileProcessingService
                        .WriteToFileAsync(file.Target, transformedSourceString).Result;
                }
            });
        }

        private async ValueTask PerformAppendOpperationsAsync(
            List<Append> appends,
            TemplateGenerationInfo templateGenerationInfo)
        {
            foreach (Append append in appends)
            {
                string fileContent = this.fileProcessingService.ReadFromFileAsync(append.Target).Result;

                string appendedContent = this.templateProcessingService.AppendContentAsync(
                    sourceContent: fileContent,
                    doesNotContainContent: append.DoesNotContainContent,
                    regexToMatchForAppend: append.RegexToMatchForAppend,
                    appendContent: append.ContentToAppend,
                    appendToBeginning: append.AppendToBeginning,
                    appendEvenIfContentAlreadyExist: append.AppendEvenIfContentAlreadyExist).Result;

                string transformedAppendedContent =
                    this.templateProcessingService.TransformStringAsync(
                        content: appendedContent,
                        replacementDictionary: templateGenerationInfo.ReplacementDictionary).Result;

                var result = await this.fileProcessingService
                    .WriteToFileAsync(append.Target, transformedAppendedContent);
            }
        }

        private void PerformExecutions(
            List<Models.Foundations.Executions.Execution> executions,
            string executionFolder,
            TemplateGenerationInfo templateGenerationInfo)
        {
            if (templateGenerationInfo.ScriptExecutionIsEnabled == true)
            {
                string outcome = this.executionProcessingService.RunAsync(executions, executionFolder).Result;
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

        private async ValueTask<List<Template>> GetOnlyTheTemplatesThatRequireGeneratingCodeAsync(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            List<Template> requiredTemplates = new List<Template>();

            foreach (Template template in templates)
            {
                template.ReplacementDictionary = replacementDictionary;

                var transformedTemplate =
                    await this.templateProcessingService.TransformTemplateAsync(template);

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
                    bool fileExist = this.fileProcessingService.CheckIfFileExistsAsync(file.Target).Result;
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

        private void LogMessage(DateTimeOffset date, string message)
        {
            this.OnProcessed(
                new ProcessedEventArgs
                {
                    TimeStamp = date,
                    Message = message,
                    Status = Status.Information,
                    ProcessedItems = this.processedItems,
                    TotalItems = this.totalItems,
                });
        }

        protected virtual void OnProcessed(ProcessedEventArgs e)
        {
            EventHandler<ProcessedEventArgs> handler = Processed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
