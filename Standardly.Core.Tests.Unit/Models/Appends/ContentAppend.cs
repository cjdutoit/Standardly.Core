// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Tests.Unit.Models.Appends
{
    public class ContentAppend
    {
        public string SourceContent { get; set; }
        public string RegexToMatch { get; set; }
        public string AppendContent { get; set; }
        public bool AppendToBeginning { get; set; } = false;
        public bool OnlyAppendIfNotPresent { get; set; } = true;
        public string AppendResult { get; set; }
    }
}
