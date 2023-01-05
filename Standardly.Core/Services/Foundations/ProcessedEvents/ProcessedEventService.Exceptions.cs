// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventService
    {
        private delegate void ReturningNothingFunction();

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (NullProcessedEventHandlerException nullProcessedEventHandler)
            {
                throw CreateAndLogValidationException(nullProcessedEventHandler);
            }
            catch (Exception exception)
            {
                var failedProcessedEventServiceException =
                    new FailedProcessedEventServiceException(exception);

                throw CreateAndLogServiceException(failedProcessedEventServiceException);
            }
        }

        private ProcessedEventValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventValidationException =
                new ProcessedEventValidationException(exception);

            return processedEventValidationException;
        }

        private ProcessedEventServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var processedEventServiceException = new ProcessedEventServiceException(exception);

            return processedEventServiceException;
        }
    }
}
