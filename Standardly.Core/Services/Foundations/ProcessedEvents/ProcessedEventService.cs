// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Events;
using Standardly.Core.Models.Foundations.ProcessedEvents;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventService : IProcessedEventService
    {
        private readonly IEventBroker eventBroker;

        public ProcessedEventService(IEventBroker eventBroker) =>
            this.eventBroker = eventBroker;

        public void ListenToProcessedEvent(
            Func<Processed, ValueTask<Processed>> processedEventHandler) =>
                TryCatch(() =>
                {
                    ValidateProcessedEventHandler(processedEventHandler);
                    this.eventBroker.ListenToProcessedEvent(processedEventHandler);
                });
    }
}
