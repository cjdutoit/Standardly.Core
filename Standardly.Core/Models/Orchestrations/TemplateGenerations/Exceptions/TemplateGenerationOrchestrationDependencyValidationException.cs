// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationOrchestrationDependencyValidationException : Xeption
    {
        public TemplateGenerationOrchestrationDependencyValidationException(Xeption innerException)
            : base(message: "Template orchestration dependency validation occurred, please try again.", innerException)
        { }
    }
}
