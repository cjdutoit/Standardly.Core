// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Events.ProcessedStatuses.Exceptions
{
    public class ProcessedStatusEventServiceException : Xeption
    {
        public ProcessedStatusEventServiceException(Exception innerException)
            : base(
                message: "Processed status event service error occurred, please contact support",
                innerException)
        { }
    }
}
