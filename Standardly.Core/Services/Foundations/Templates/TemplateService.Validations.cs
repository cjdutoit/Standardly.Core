// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates.Exceptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private static void ValidateTransformString(
            string content,
            Dictionary<string, string> replacementDictionary)
        {
            Validate(
                (Rule: IsInvalid(content), Parameter: nameof(content)),
                (Rule: IsInvalid(replacementDictionary), Parameter: nameof(replacementDictionary)));
        }

        private static void ValidateTransformationArguments(
            string content,
            char tagCharacter)
        {
            Validate(
                (Rule: IsInvalid(content), Parameter: nameof(content)),
                (Rule: IsInvalid(tagCharacter), Parameter: nameof(tagCharacter)));
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


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTemplateException =
                new InvalidArgumentTemplateException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTemplateException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTemplateException.ThrowIfContainsErrors();
        }
    }
}
