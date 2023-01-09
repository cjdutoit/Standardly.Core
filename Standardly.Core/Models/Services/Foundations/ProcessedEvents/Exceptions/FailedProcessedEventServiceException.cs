// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.ProcessedEvents.Exceptions
{
    public class FailedProcessedEventServiceException : Xeption
    {
        public FailedProcessedEventServiceException(Exception innerException)
            : base(
                message: "Failed processed event service error occurred, please contact support",
                innerException)
        { }
    }
}
