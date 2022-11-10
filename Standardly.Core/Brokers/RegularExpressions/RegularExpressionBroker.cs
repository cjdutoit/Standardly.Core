// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Standardly.Core.Brokers.RegularExpressions
{
    public class RegularExpressionBroker : IRegularExpressionBroker
    {
        public (bool matchFound, string matchedContent) CheckForExpressionMatch(string regexToMatch, string sourceContent)
        {
            Regex regex = new Regex(regexToMatch, RegexOptions.Multiline);
            Match match = regex.Match(sourceContent);
            bool matchFound = match.Success;
            return (matchFound, match.Value);
        }

        public string Replace(string sourceContent, string regexToMatch, string replaceMatchWithNewContent)
        {
            return Regex.Replace(sourceContent, regexToMatch, replaceMatchWithNewContent);
        }
    }
}
