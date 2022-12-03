// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Events;
using Standardly.Core.Models.Orchestrations;

namespace Standardly.Core.Services.Orchestrations.TemplatesGenerations
{
    public interface ITemplateGenerationOrchestrationService
    {
        event EventHandler<ProcessedEventArgs> Processed;
        void GenerateCode(TemplateGenerationInfo templateGenerationInfo);
    }
}
