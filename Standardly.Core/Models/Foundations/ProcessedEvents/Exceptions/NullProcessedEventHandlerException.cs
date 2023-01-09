// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions
{
    public class NullProcessedEventHandlerException : Xeption
    {
        public NullProcessedEventHandlerException()
            : base(message: "Processed event handler is null.")
        { }
    }
}
