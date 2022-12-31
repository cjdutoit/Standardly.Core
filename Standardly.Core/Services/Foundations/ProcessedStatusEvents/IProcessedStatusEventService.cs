// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Events;

namespace Standardly.Core.Services.Foundations.ProcessedStatusEvents
{
    public interface IProcessedStatusEventService
    {
        void ListenToProcessedStatusEvent(Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler);
        ValueTask<ProcessedStatus> PublishProcessedStatusAsync(ProcessedStatus status);
    }
}
