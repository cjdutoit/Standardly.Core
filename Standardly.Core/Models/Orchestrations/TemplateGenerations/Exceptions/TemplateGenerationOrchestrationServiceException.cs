// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationOrchestrationServiceException : Xeption
    {
        public TemplateGenerationOrchestrationServiceException(Exception innerException)
            : base(message: "Template orchestration service error occurred, contact support.", innerException)
        { }
    }
}
