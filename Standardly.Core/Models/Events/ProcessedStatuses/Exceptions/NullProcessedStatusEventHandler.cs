// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Events.ProcessedStatuses.Exceptions
{
    public class NullProcessedStatusEventHandler : Xeption
    {
        public NullProcessedStatusEventHandler()
            : base(message: "Process status event handler is null.")
        { }
    }
}
