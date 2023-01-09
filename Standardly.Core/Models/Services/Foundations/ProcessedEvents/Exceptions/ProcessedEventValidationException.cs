// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.ProcessedEvents.Exceptions
{
    public class ProcessedEventValidationException : Xeption
    {
        public ProcessedEventValidationException(Xeption innerException)
            : base(
                message: "Processed event validation error occurred, please try again.",
                innerException)
        { }
    }
}
