// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public interface IProcessedEventService
    {
        void ListenToProcessedEvent(Func<Processed, ValueTask<Processed>> processedEventHandler);
        ValueTask PublishProcessedAsync(Processed processed);
    }
}
