﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Events;
using Standardly.Core.Models.Events;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public class ProcessedEventService : IProcessedEventService
    {
        private readonly IEventBroker eventBroker;

        public ProcessedEventService(IEventBroker eventBroker) =>
            this.eventBroker = eventBroker;

        public void ListenToProcessedEvent(Func<Processed, ValueTask<Processed>> processedEventHandler) =>
            throw new NotImplementedException();
    }
}
