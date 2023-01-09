// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.ProcessedEvents.Exceptions
{
    public class NullProcessedEventException : Xeption
    {
        public NullProcessedEventException()
            : base(message: "Processed status event handler is null.")
        { }
    }
}
