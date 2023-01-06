// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Orchestrations.Operations.Exceptions
{
    public class FailedOperationOrchestrationServiceException : Xeption
    {
        public FailedOperationOrchestrationServiceException(Exception innerException)
            : base(message: "Failed operation orchestration service occurred, please contact support", innerException)
        { }
    }
}
