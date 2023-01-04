// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.ProcessedEvents;

namespace Standardly.Core.Brokers.Events
{
    public partial class EventBroker : IEventBroker
    {
        private static Func<Processed, ValueTask<Processed>> ProcessedEventHandler;

        public void ListenToProcessedEvent(Func<Processed, ValueTask<Processed>> processedEventHandler) =>
            ProcessedEventHandler = processedEventHandler;

        public async ValueTask PublishProcessedEventAsync(Processed processed) =>
            await ProcessedEventHandler(processed);
    }
}
