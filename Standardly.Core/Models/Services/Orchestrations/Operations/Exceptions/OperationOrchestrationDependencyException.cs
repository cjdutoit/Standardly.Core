// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions
{
    public class OperationOrchestrationDependencyException : Xeption
    {
        public OperationOrchestrationDependencyException(Xeption innerException)
            : base(
                  message: "Operation orchestration dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
