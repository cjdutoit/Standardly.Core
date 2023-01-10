// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Processings.ProcessedEvents;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplateGenerations
{
    public class TemplateGenerationOrchestrationService : ITemplateGenerationOrchestrationService
    {
        private readonly IProcessedEventProcessingService processedEventProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;

        public TemplateGenerationOrchestrationService(
            IProcessedEventProcessingService processedEventProcessingService,
            ITemplateProcessingService templateProcessingService)
        {
            this.processedEventProcessingService = processedEventProcessingService;
            this.templateProcessingService = templateProcessingService;
        }

        public void ListenToProcessedEvent(
            Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>> processedEventOrchestrationHandler)
        {
            this.processedEventProcessingService.ListenToProcessedEvent(async (processed) =>
                {
                    TemplateGenerationInfo templateGenerationInfo = MapToTemplateGenerationInfo(processed);
                    await processedEventOrchestrationHandler(templateGenerationInfo);

                    return await Task.FromResult(processed);
                });
        }

        public ValueTask PublishProcessedAsync(TemplateGenerationInfo processed) =>
            throw new NotImplementedException();

        public ValueTask<TemplateGenerationInfo> ConvertStringToTemplateAsync(string content) =>
            throw new NotImplementedException();

        public ValueTask<TemplateGenerationInfo> TransformTemplateAsync(TemplateGenerationInfo template) =>
            throw new NotImplementedException();

        public ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary) =>
                throw new NotImplementedException();

        public ValueTask<string> AppendContentAsync(
            string sourceContent,
            string doesNotContainContent,
            string regexToMatchForAppend,
            string appendContent,
            bool appendToBeginning,
            bool appendEvenIfContentAlreadyExist) =>
                throw new NotImplementedException();

        private TemplateGenerationInfo MapToTemplateGenerationInfo(Processed processed)
        {
            TemplateGenerationInfo templateGenerationInfo = new TemplateGenerationInfo
            {
                Processed = processed
            };

            return templateGenerationInfo;
        }
    }
}
