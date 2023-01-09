// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.ProcessedEvents;

namespace Standardly.Core.Services.Processings.ProcessedEvents
{
    public interface IProcessedEventProcessingService
    {
        void ListenToProcessedEvent(
            Func<Processed, ValueTask<Processed>> processedEventProcessingHandler);

        ValueTask PublishProcessedAsync(Processed processed);
    }
}
