// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Orchestrations.Operations.Exceptions
{
    public class InvalidArgumentOperationOrchestrationException : Xeption
    {
        public InvalidArgumentOperationOrchestrationException()
            : base(message: "Invalid operation orchestration argument(s), please correct the errors and try again.")
        { }
    }
}
