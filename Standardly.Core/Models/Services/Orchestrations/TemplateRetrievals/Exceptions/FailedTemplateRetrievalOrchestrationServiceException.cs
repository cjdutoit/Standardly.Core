// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions
{
    public class FailedTemplateRetrievalOrchestrationServiceException : Xeption
    {
        public FailedTemplateRetrievalOrchestrationServiceException(Exception innerException)
            : base(message: "Failed template orchestration service occurred, please contact support", innerException)
        { }
    }
}
