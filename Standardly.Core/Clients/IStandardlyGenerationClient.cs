// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations;

namespace Standardly.Core.Clients
{
    public interface IStandardlyGenerationClient
    {
        event Action<DateTimeOffset, string, string> LogRaised;
        void GenerateCode(TemplateGenerationInfo templateGenerationInfo);
    }
}
