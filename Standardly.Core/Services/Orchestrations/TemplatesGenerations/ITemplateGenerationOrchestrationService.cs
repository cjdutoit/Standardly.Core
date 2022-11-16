﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplatesGenerations
{
    public interface ITemplateGenerationOrchestrationService
    {
        event Action<DateTimeOffset, string, string> LogRaised;
        bool ScriptExecutionIsEnabled { get; set; }
        void GenerateCode(List<Template> templates, Dictionary<string, string> replacementDictionary);
    }
}