// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions
{
    public class InvalidArgumentTemplateRetrievalOrchestrationException : Xeption
    {
        public InvalidArgumentTemplateRetrievalOrchestrationException()
            : base(message: "Invalid template argument(s), please correct the errors and try again.")
        { }
    }
}
