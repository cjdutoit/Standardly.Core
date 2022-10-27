﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Processings.Executions.Exceptions
{
    public class ExecutionProcessingServiceException : Xeption
    {
        public ExecutionProcessingServiceException(Xeption innerException)
            : base(message: "Execution processing service error occurred, please contact support", innerException)
        { }
    }
}
