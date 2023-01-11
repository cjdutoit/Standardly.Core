// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions
{
    public class FailedProcessedEventOrchestrationServiceException : Xeption
    {
        public FailedProcessedEventOrchestrationServiceException(Exception innerException)
            : base(
                message: "Failed processed event orchestration service error occurred, please contact support",
                innerException)
        { }
    }
}
