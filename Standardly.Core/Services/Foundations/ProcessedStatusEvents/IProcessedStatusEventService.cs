// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Events.ProcessedStatuses;

namespace Standardly.Core.Services.Foundations.ProcessedStatusEvents
{
    public interface IProcessedStatusEventService
    {
        void ListenToProcessedStatusEvent(Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler);
        ValueTask PublishProcessedStatusAsync(ProcessedStatus status);
    }
}
