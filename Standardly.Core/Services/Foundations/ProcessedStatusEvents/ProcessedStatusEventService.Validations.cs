// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Events.ProcessedStatuses;
using Standardly.Core.Models.Events.ProcessedStatuses.Exceptions;

namespace Standardly.Core.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventService
    {
        private void ValidateProcessedEventHandler(
            Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler)
        {
            ValidateProcessedEventHandlerIsNotNull(processedEventHandler);
        }

        private void ValidateProcessedStatusOnPublish(ProcessedStatus processedStatus)
        {
            ValidateProcessedStatusIsNotNull(processedStatus);
        }

        private static void ValidateProcessedEventHandlerIsNotNull(
            Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedEventHandler)
        {
            if (processedEventHandler is null)
            {
                throw new NullProcessedStatusEventHandler();
            }
        }

        private static void ValidateProcessedStatusIsNotNull(ProcessedStatus processedStatus)
        {
            if (processedStatus is null)
            {
                throw new NullProcessedStatusException();
            }
        }
    }
}
