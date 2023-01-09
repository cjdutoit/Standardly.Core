// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventService
    {
        private void ValidateProcessedEventHandler(
            Func<Processed, ValueTask<Processed>> processedEventHandler)
        {
            ValidateProcessedEventHandlerIsNotNull(processedEventHandler);
        }

        private void ValidateProcessedOnPublish(Processed processed)
        {
            ValidateProcessedIsNotNull(processed);
        }

        private static void ValidateProcessedEventHandlerIsNotNull(
            Func<Processed, ValueTask<Processed>> processedEventHandler)
        {
            if (processedEventHandler is null)
            {
                throw new NullProcessedEventHandlerException();
            }
        }

        private static void ValidateProcessedIsNotNull(Processed processed)
        {
            if (processed is null)
            {
                throw new NullProcessedEventException();
            }
        }
    }
}
