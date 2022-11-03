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

        private static void ValidateTransformationArguments(
            string content,
            char tagCharacter)
        {
            Validate<InvalidArgumentTemplateException>(
                (Rule: IsInvalid(content), Parameter: nameof(content)),
                (Rule: IsInvalid(tagCharacter), Parameter: nameof(tagCharacter)));
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
                }
            }

            return actionRules;
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

        private void CheckAllTagsHasBeenReplaced(string template, char tagCharacter = '$')
        {
            var regex = $@"\{tagCharacter}([a-zA-Z]*)\{tagCharacter}";
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
