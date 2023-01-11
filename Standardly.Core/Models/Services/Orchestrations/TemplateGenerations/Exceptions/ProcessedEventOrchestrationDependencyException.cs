// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions
{
    public class ProcessedEventOrchestrationDependencyException : Xeption
    {
        public ProcessedEventOrchestrationDependencyException(Xeption innerException) :
            base(message: "Processed event orchestration dependency error occurred, contact support.", innerException)
        { }
    }
}
