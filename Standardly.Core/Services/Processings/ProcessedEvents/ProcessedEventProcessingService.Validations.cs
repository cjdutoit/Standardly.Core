// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.ProcessedEvents.Exceptions;

namespace Standardly.Core.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingService : IProcessedEventProcessingService
    {
        private void ValidateProcessedEventProcessingHandler(
            Func<Processed, ValueTask<Processed>> processedEventProcessingHandler)
        {
            if (processedEventProcessingHandler is null)
            {
                throw new NullProcessedEventProcessingHandler();
            }
        }

        private void ValidateProcessedOnPublish(Processed processed)
        {
            if (processed is null)
            {
                throw new NullProcessedEventProcessingException();
            }
        }
    }
}
