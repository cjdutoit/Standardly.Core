// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private static void ValidateTransformString(
            string content,
            Dictionary<string, string> replacementDictionary)
        {
            Validate<InvalidArgumentTemplateException>(
                (Rule: IsInvalid(content), Parameter: nameof(content)),
                (Rule: IsInvalid(replacementDictionary), Parameter: nameof(replacementDictionary)));
        }

        private static void ValidateTransformationArguments(string content)
        {
            Validate<InvalidArgumentTemplateException>(
                (Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private static void ValidateConvertStringToTemplateArguments(string content)
        {
            Validate<InvalidArgumentTemplateException>((Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private void ValidateTemplate(Template template)
        {
            var templateRules = new List<(dynamic Rule, string Parameter)>()
            {
                (Rule: IsInvalid(template.Name), Parameter: "Template Name"),
                (Rule: IsInvalid(template.Description), Parameter: "Template Description"),
                (Rule: IsInvalid(template.TemplateType), Parameter: "Template Type"),
                (Rule: IsInvalid(template.ProjectsRequired), Parameter: "Template Projects Required"),
                (Rule: IsInvalid(template.Tasks), Parameter: "Template Tasks")
            };

            templateRules.AddRange(GetTaskValidationRules(template));
            Validate<InvalidTemplateException>(templateRules.ToArray());
        }

        private List<(dynamic Rule, string Parameter)> GetTaskValidationRules(Template template)
        {
            var taskRules = new List<(dynamic Rule, string Parameter)>();

            if (template.Tasks.Any())
            {
                var tasks = template.Tasks;

                for (int taskIndex = 0; taskIndex <= tasks.Count - 1; taskIndex++)
                {
                    taskRules.Add((Rule: IsInvalid(tasks[taskIndex].Name), Parameter: $"Tasks[{taskIndex}].Name"));

                    taskRules.Add(
                        (Rule: IsInvalid(tasks[taskIndex].Actions), Parameter: $"Tasks[{taskIndex}].Actions"));

                    taskRules.AddRange(GetActionValidationRules(tasks[taskIndex]));
                }
            }

            return taskRules;
        }

        private List<(dynamic Rule, string Parameter)> GetActionValidationRules(
            Models.Foundations.Templates.Tasks.Task task)
        {
            var actionRules = new List<(dynamic Rule, string Parameter)>();

            if (task.Actions.Any())
            {
                var actions = task.Actions;

                for (int actionIndex = 0; actionIndex <= actions.Count - 1; actionIndex++)
                {
                    actionRules.Add(
                        (Rule: IsInvalid(actions[actionIndex].Name),
                                Parameter: $"Actions[{actionIndex}].Name"));

                    actionRules.Add(
                        (Rule: IsInvalid(actions[actionIndex].Executions),
                            Parameter: $"Actions[{actionIndex}].Executions"));

                    actionRules.AddRange(GetFileValidationRules(actions[actionIndex], actionIndex));
                    actionRules.AddRange(GetAppendValidationRules(actions[actionIndex], actionIndex));
                    actionRules.AddRange(GetExecutionValidationRules(actions[actionIndex], actionIndex));
                }
            }

            return actionRules;
        }

        private List<(dynamic Rule, string Parameter)> GetFileValidationRules(
            Models.Foundations.Templates.Tasks.Actions.Action action, int actionIndex)
        {
            var fileItemRules = new List<(dynamic Rule, string Parameter)>();

            if (action.Files.Any())
            {
                var files = action.Files;

                for (int fileItemIndex = 0; fileItemIndex <= files.Count - 1; fileItemIndex++)
                {
                    fileItemRules.Add(
                        (Rule: IsInvalid(files[fileItemIndex].Template),
                            Parameter: $"Actions[{actionIndex}].Files[{fileItemIndex}].Template"));

                    fileItemRules.Add(
                        (Rule: IsInvalid(files[fileItemIndex].Target),
                            Parameter: $"Actions[{actionIndex}].Files[{fileItemIndex}].Target"));
                }
            }

            return fileItemRules;
        }

        private List<(dynamic Rule, string Parameter)> GetAppendValidationRules(
            Models.Foundations.Templates.Tasks.Actions.Action action, int actionIndex)
        {
            var appendRules = new List<(dynamic Rule, string Parameter)>();

            if (action.Appends.Any())
            {
                var appends = action.Appends;

                for (int fileItemIndex = 0; fileItemIndex <= appends.Count - 1; fileItemIndex++)
                {
                    appendRules.Add(
                        (Rule: IsInvalid(appends[fileItemIndex].Target),
                            Parameter: $"Actions[{actionIndex}].Appends[{fileItemIndex}].Target"));

                    appendRules.Add(
                        (Rule: IsInvalid(appends[fileItemIndex].RegexToMatch),
                            Parameter: $"Actions[{actionIndex}].Appends[{fileItemIndex}].RegexToMatch"));

                    appendRules.Add(
                        (Rule: IsInvalid(appends[fileItemIndex].ContentToAppend),
                            Parameter: $"Actions[{actionIndex}].Appends[{fileItemIndex}].ContentToAppend"));
                }
            }

            return appendRules;
        }

        private List<(dynamic Rule, string Parameter)> GetExecutionValidationRules(
            Models.Foundations.Templates.Tasks.Actions.Action action, int actionIndex)
        {
            var executionRules = new List<(dynamic Rule, string Parameter)>();

            if (action.Executions.Any())
            {
                var executions = action.Executions;

                for (int executionIndex = 0; executionIndex <= executions.Count - 1; executionIndex++)
                {
                    executionRules.Add(
                        (Rule: IsInvalid(executions[executionIndex].Name),
                            Parameter: $"Actions[{actionIndex}].Executions[{executionIndex}].Name"));

                    executionRules.Add(
                        (Rule: IsInvalid(executions[executionIndex].Instruction),
                            Parameter: $"Actions[{actionIndex}].Executions[{executionIndex}].Instruction"));
                }
            }

            return executionRules;
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(char tagCharacter) => new
        {
            Condition = String.IsNullOrWhiteSpace(tagCharacter.ToString().Replace("\0", "")),
            Message = "Character is required"
        };

        private static dynamic IsInvalid(Dictionary<string, string> dictionary) => new
        {
            Condition = dictionary == null,
            Message = "Dictionary is required"
        };

        private static dynamic IsInvalid(List<Models.Foundations.Templates.Tasks.Task> tasks) => new
        {
            Condition = tasks.Count == 0,
            Message = "Tasks is required"
        };

        private static dynamic IsInvalid(List<Models.Foundations.Templates.Tasks.Actions.Action> actions) => new
        {
            Condition = actions.Count == 0,
            Message = "Actions is required"
        };

        private static dynamic IsInvalid(List<Execution> executions) => new
        {
            Condition = executions.Count == 0,
            Message = "Executions is required"
        };

        private void CheckAllTagsHasBeenReplaced(string template)
        {
            var regex = $@"\$([a-zA-Z]*)\$";
            var matches = Regex.Matches(template, regex);
            List<string> tags = new List<string>();

            foreach (Match match in matches)
            {
                if (!tags.Contains(match.Value))
                {
                    tags.Add(match.Value);
                }
            }

            var invalidReplacementException = new InvalidReplacementException();

            foreach (string tag in tags)
            {
                invalidReplacementException.UpsertDataList(
                    key: tag,
                    value: $"Found '{tag}' that was not in the replacement dictionary, " +
                        $"fix the errors and try again.");
            }

            invalidReplacementException.ThrowIfContainsErrors();
        }


        private static void Validate<T>(params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidException = (Xeption)Activator.CreateInstance(typeof(T));

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidException.ThrowIfContainsErrors();
        }
    }
}
