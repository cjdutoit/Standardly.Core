// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;

namespace Standardly.Core.Services.Orchestrations.TemplateGenerations
{
    public interface ITemplateGenerationOrchestrationService
    {
        void ListenToProcessedEvent(
            Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>> processedEventProcessingHandler);

        ValueTask PublishProcessedAsync(TemplateGenerationInfo templateGenerationInfo);

        ValueTask<TemplateGenerationInfo> ConvertStringToTemplateAsync(string content);

        ValueTask<TemplateGenerationInfo> TransformTemplateAsync(TemplateGenerationInfo template);

        ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary);

        ValueTask<string> AppendContentAsync(
            string sourceContent,
            string doesNotContainContent,
            string regexToMatchForAppend,
            string appendContent,
            bool appendToBeginning,
            bool appendEvenIfContentAlreadyExist);
    }
}
