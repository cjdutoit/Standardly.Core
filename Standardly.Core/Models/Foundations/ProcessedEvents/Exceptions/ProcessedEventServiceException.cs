// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions
{
    public class ProcessedEventServiceException : Xeption
    {
        public ProcessedEventServiceException(Exception innerException)
            : base(
                message: "Processed event service error occurred, please contact support",
                innerException)
        { }
    }
}
