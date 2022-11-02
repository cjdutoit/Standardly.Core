// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates.Exceptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private static void ValidateTransformString(string content,
            Dictionary<string, string> replacementDictionary)
        {
            Validate((Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTemplateException =
                new InvalidTemplateException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTemplateException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTemplateException.ThrowIfContainsErrors();
        }
    }
}
