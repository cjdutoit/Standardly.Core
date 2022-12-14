// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions
{
    public class TemplateRetrievalOrchestrationValidationException : Xeption
    {
        public TemplateRetrievalOrchestrationValidationException(Xeption innerException)
            : base(message: "Template orcgestration validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
