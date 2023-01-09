// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions
{
    public class ProcessedEventProcessingDependencyException : Xeption
    {
        public ProcessedEventProcessingDependencyException(Xeption innerException)
            : base(
                  message: "Processed event processing dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
