// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Orchestrations.Operations.Exceptions
{
    public class OperationOrchestrationDependencyValidationException : Xeption
    {
        public OperationOrchestrationDependencyValidationException(Xeption innerException)
            : base(message: "Operation orchestration dependency validation error occurred, please try again.",
                  innerException)
        { }
    }
}
