// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Processings.ProcessedEvents.Exceptions
{
    public class ProcessedEventProcessingDependencyValidationException : Xeption
    {
        public ProcessedEventProcessingDependencyValidationException(Xeption innerException)
            : base(
                  message: "Processed event dependency validation error occurred, please try again.",
                  innerException)
        { }
    }
}
