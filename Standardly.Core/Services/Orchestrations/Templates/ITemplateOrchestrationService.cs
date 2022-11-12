// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public interface ITemplateOrchestrationService
    {
        event Action<DateTimeOffset, string, string> LogRaised;
        bool ScriptExecutionIsEnabled { get; set; }
        ValueTask<List<Template>> FindAllTemplatesAsync();
        ValueTask GenerateCodeAsync(List<Template> templates, Dictionary<string, string> replacementDictionary);
    }
}
