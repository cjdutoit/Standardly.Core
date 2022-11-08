// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Brokers.RegularExpressions
{
    public interface IRegularExpressionBroker
    {
        (bool matchFound, string match) CheckForExpressionMatch(string regexToMatch, string sourceContent);
        string Replace(string sourceContent, string regexToMatch, string replaceMatchWithNewContent);
    }
}
