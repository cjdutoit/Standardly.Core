// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Events;
using Standardly.Core.Models.Events.ProcessedStatuses;

namespace Standardly.Core.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventService : IProcessedStatusEventService
    {
        private readonly IEventBroker eventBroker;

        public ProcessedStatusEventService(IEventBroker eventBroker) =>
            this.eventBroker = eventBroker;

        public void ListenToProcessedStatusEvent(
            Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler) =>
                TryCatch(() =>
                {
                    ValidateProcessedEventHandler(processedEventHandler);
                    this.eventBroker.ListenToProcessedEvent(processedEventHandler);
                });

        public ValueTask PublishProcessedStatusAsync(ProcessedStatus processedStatus) =>
            TryCatch(async () =>
            {
                ValidateProcessedStatusOnPublish(processedStatus);
                await this.eventBroker.PublishProcessedEventAsync(processedStatus);
            });
    }
}
