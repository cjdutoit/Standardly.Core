// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Events;
using Standardly.Core.Models.Events.ProcessedStatuses;

namespace Standardly.Core.Brokers.Events
{
    public partial class EventBroker : IEventBroker
    {
        private static Func<ProcessedStatus, ValueTask<ProcessedStatus>> ProcessedEventHandler;

        public void ListenToProcessedEvent(Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler) =>
            ProcessedEventHandler = processedEventHandler;

        public async ValueTask PublishProcessedEventAsync(ProcessedStatus processed) =>
            await ProcessedEventHandler(processed);
    }
}
