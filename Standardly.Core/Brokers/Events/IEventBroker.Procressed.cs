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
    public partial interface IEventBroker
    {
        void ListenToProcessedEvent(Func<Processed, ValueTask<Processed>> processedEventHandler);
        ValueTask PublishProcessedEventAsync(Processed processed);
    }
}
