// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace Standardly.Core.Models.Events
{
    public class ProcessedEventArgs : EventArgs
    {
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int ProcessedItems { get; set; }
        public int TotalItems { get; set; }
    }
}
