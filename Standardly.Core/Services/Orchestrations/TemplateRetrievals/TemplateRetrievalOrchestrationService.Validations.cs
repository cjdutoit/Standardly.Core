// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Orchestrations.TemplateRetrievals.Exceptions;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationService
    {
        private static void ValidateFindTemplateArguments(
            string templateFolderPath,
            string templateDefinitionFileName)
        {
            Validate(
                (Rule: IsInvalid(templateFolderPath), Parameter: nameof(templateFolderPath)),
                (Rule: IsInvalid(templateDefinitionFileName), Parameter: nameof(templateDefinitionFileName)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTemplateRetrievalOrchestrationException =
                new InvalidArgumentTemplateRetrievalOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTemplateRetrievalOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTemplateRetrievalOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
