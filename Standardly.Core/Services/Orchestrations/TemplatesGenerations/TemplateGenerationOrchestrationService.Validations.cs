// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.EntityModels;
using Standardly.Core.Models.Orchestrations;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Templates.Exceptions;

namespace Standardly.Core.Services.Orchestrations.TemplatesGenerations
{
    public partial class TemplateGenerationOrchestrationService
    {
        private static void ValidateTemplateGenerationInfoIsNotNull(
            TemplateGenerationInfo templateGenerationInfo)
        {
            if (templateGenerationInfo == null)
            {
                throw new NullTemplateGenerationOrchestrationException();
            }
        }

        private static void ValidateTemplateArguments(TemplateGenerationInfo templateGenerationInfo)
        {
            Validate(
                (Rule: IsInvalid(templateGenerationInfo.Templates),
                    Parameter: nameof(templateGenerationInfo.Templates)),

                (Rule: IsInvalid(templateGenerationInfo.ReplacementDictionary),
                    Parameter: nameof(templateGenerationInfo.ReplacementDictionary)),

                (Rule: IsInvalid(templateGenerationInfo.EntityModelDefinition),
                    Parameter: nameof(templateGenerationInfo.EntityModelDefinition)));
        }

        private static dynamic IsInvalid(List<Template> templates) => new
        {
            Condition = templates == null,
            Message = "Templates is required"
        };

        private static dynamic IsInvalid(Dictionary<string, string> replacementDictionary) => new
        {
            Condition = replacementDictionary == null,
            Message = "Dictionary is required"
        };

        private static dynamic IsInvalid(List<EntityModel> entityModelDefinition) => new
        {
            Condition = entityModelDefinition == null,
            Message = "Dictionary is required"
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
