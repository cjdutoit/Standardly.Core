// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateGenerationOrchestrationService
    {
        private static void ValidateTemplateArguments(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            Validate(
                (Rule: IsInvalid(templates), Parameter: nameof(templates)),
                (Rule: IsInvalid(replacementDictionary), Parameter: nameof(replacementDictionary)));
        }

        private static dynamic IsInvalid(List<Template> templates) => new
        {
            Condition = templates == null,
            Message = "Templates is required"
        };

        private static dynamic IsInvalid(Dictionary<string, string> replacementDictionary) => new
        {
            Condition = replacementDictionary == null,
            Message = "Dictionary values is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTemplateOrchestrationException =
                new InvalidArgumentTemplateGenerationOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTemplateOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTemplateOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
