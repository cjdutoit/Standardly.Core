// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Services.Foundations.ProcessedEvents;

namespace Standardly.Core.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingService : IProcessedEventProcessingService
    {
        private readonly IProcessedEventService processedEventService;

        public ProcessedEventProcessingService(IProcessedEventService processedEventService) =>
            this.processedEventService = processedEventService;

        public void ListenToProcessedEvent(Func<Processed, ValueTask<Processed>> processedEventProcessingHandler) =>
                TryCatch(() =>
                {
                    ValidateProcessedEventProcessingHandler(processedEventProcessingHandler);

                    this.processedEventService.ListenToProcessedEvent(processedEventProcessingHandler);
                });

        public ValueTask PublishProcessedAsync(Processed processed) =>
            TryCatch(async () =>
            {
                ValidateProcessedOnPublish(processed);
                await this.processedEventService.PublishProcessedAsync(processed);
            });

    }
}
