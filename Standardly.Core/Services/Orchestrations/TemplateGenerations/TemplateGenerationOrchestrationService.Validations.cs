// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions;

namespace Standardly.Core.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationService : ITemplateGenerationOrchestrationService
    {
        private void ValidateProcessedEventOrchestrationHandler(
            Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>> processedEventOrchestrationHandler)
        {
            if (processedEventOrchestrationHandler is null)
            {
                throw new NullProcessedEventOrchestrationHandlerException();
            }
        }
    }
}
