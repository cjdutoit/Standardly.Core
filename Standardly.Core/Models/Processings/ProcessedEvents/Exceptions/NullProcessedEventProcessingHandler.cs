// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Processings.ProcessedEvents.Exceptions
{
    public class NullProcessedEventProcessingHandler : Xeption
    {
        public NullProcessedEventProcessingHandler()
            : base(message: "Processed event processing handler is null.")
        { }
    }
}
