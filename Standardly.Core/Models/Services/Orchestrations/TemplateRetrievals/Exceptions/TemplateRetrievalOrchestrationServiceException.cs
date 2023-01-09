// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions
{
    public class TemplateRetrievalOrchestrationServiceException : Xeption
    {
        public TemplateRetrievalOrchestrationServiceException(Exception innerException)
            : base(message: "Template orchestration service error occurred, contact support.", innerException)
        { }
    }
}
