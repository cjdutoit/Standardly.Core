// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Clients
{
    public interface IStandardlyClient
    {
        event Action<DateTimeOffset, string, string> LogRaised;
        ValueTask<List<Template>> FindAllTemplatesAsync();
    }
}
