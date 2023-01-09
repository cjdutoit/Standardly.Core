// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions
{
    public class OperationOrchestrationServiceException : Xeption
    {
        public OperationOrchestrationServiceException(Xeption innerException)
            : base(message: "Operation orchestration service error occurred, please contact support", innerException)
        { }
    }
}
