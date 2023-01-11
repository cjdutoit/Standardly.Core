// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions
{
    public class ProcessedEventOrchestrationServiceException : Xeption
    {
        public ProcessedEventOrchestrationServiceException(Exception innerException)
            : base(
                message: "Processed event orchestration service error occurred, please contact support",
                innerException)
        { }
    }
}
