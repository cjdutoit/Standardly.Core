﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Processings.Templates.Exceptions;

namespace Standardly.Core.Services.Processings.Templates
{
    public partial class TemplateProcessingService
    {
        private static void ValidateConvertStringToTemplate(string content)
        {
            Validate((Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private static void ValidateTransformTemplate(
            Template template,
            Dictionary<string, string> replacementDictionary)
        {
            Validate(
                (Rule: IsInvalid(template), Parameter: nameof(template)),
                (Rule: IsInvalid(replacementDictionary), Parameter: nameof(replacementDictionary)));
        }

        private static void ValidateTransformString(
            string content,
            Dictionary<string, string> replacementDictionary)
        {
            Validate(
                (Rule: IsInvalid(content), Parameter: nameof(content)),
                (Rule: IsInvalid(replacementDictionary), Parameter: nameof(replacementDictionary)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Template template) => new
        {
            Condition = template == null,
            Message = "Template is required"
        };

        private static dynamic IsInvalid(Dictionary<string, string> replacementDictionary) => new
        {
            Condition = replacementDictionary == null,
            Message = "Dictionary values is required"
        };

        private static dynamic IsInvalid(char tagCharacter) => new
        {
            Condition = String.IsNullOrWhiteSpace(tagCharacter.ToString().Replace("\0", "")),
            Message = "Character is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTemplateProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTemplateProcessingException.ThrowIfContainsErrors();
        }
    }
}
