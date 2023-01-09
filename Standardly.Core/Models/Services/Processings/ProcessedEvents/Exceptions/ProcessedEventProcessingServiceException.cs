// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions
{
    public class ProcessedEventProcessingServiceException : Xeption
    {
        public ProcessedEventProcessingServiceException(Exception innerException)
            : base(
                message: "Processed event processing service error occurred, please contact support",
                innerException)
        { }
    }
}
