// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.ProcessedEvents.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingService : IProcessedEventProcessingService
    {
        private delegate void ReturningNothingFunction();
        private delegate ValueTask ReturningValueTaskFunction();

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (NullProcessedEventProcessingHandler nullProcessedEventProcessingHandler)
            {
                throw CreateAndLogValidationException(nullProcessedEventProcessingHandler);
            }
            catch (ProcessedEventValidationException processedEventValidationException)
            {
                throw CreateAndLogDependencyValidationException(processedEventValidationException);
            }
            catch (ProcessedEventServiceException processedEventServiceException)
            {
                throw CreateAndLogDependencyException(processedEventServiceException);
            }
            catch (Exception exception)
            {
                var failedProcessedStatusProcessingServiceException =
                    new FailedProcessedEventProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedProcessedStatusProcessingServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningValueTaskFunction returningValueTaskFunction)
        {
            try
            {
                await returningValueTaskFunction();
            }
            catch (NullProcessedEventProcessingException nullProcessedEventProcessingException)
            {
                throw CreateAndLogValidationException(nullProcessedEventProcessingException);
            }
            catch (ProcessedEventValidationException processedEventValidationException)
            {
                throw CreateAndLogDependencyValidationException(processedEventValidationException);
            }
            catch (ProcessedEventServiceException processedEventServiceException)
            {
                throw CreateAndLogDependencyException(processedEventServiceException);
            }
        }

        private ProcessedEventProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventProcessingValidationException =
                new ProcessedEventProcessingValidationException(exception);

            return processedEventProcessingValidationException;
        }

        private ProcessedEventProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var processedEventProcessingDependencyValidationException =
                new ProcessedEventProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            return processedEventProcessingDependencyValidationException;
        }

        private ProcessedEventProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var processedEventProcessingDependencyException =
                new ProcessedEventProcessingDependencyException(
                    exception.InnerException as Xeption);

            return processedEventProcessingDependencyException;
        }

        private ProcessedEventProcessingServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var processedEventProcessingServiceException = new ProcessedEventProcessingServiceException(exception);

            return processedEventProcessingServiceException;
        }
    }
}
