// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventService
    {
        private delegate void ReturningNothingFunction();
        private delegate ValueTask ReturningValueTaskFunction();

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

        private async ValueTask TryCatch(ReturningValueTaskFunction returningValueTaskFunction)
        {
            try
            {
                await returningValueTaskFunction();
            }
            catch (NullProcessedEventException nullProcessedEventException)
            {
                throw CreateAndLogValidationException(nullProcessedEventException);
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
