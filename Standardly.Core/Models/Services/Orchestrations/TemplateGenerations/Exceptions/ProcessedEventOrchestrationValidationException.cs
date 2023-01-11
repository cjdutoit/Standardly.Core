// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions
{
    public class ProcessedEventOrchestrationValidationException : Xeption
    {
        public ProcessedEventOrchestrationValidationException(Exception innerException)
        : base(
            message: "Processed event orchestration service error occurred, please contact support",
            innerException)
        { }
    }
}
