// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Files
{
    public class Append
    {
        public string Target { get; set; }
        public string RegexToMatch { get; set; }
        public string ContentToAppend { get; set; }
        public bool AppendToTop { get; set; } = false;
    }
}
