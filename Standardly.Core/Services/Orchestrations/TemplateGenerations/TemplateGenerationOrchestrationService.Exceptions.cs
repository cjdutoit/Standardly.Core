// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationService : ITemplateGenerationOrchestrationService
    {
        private delegate void ReturningNothingFunction();

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (NullProcessedEventOrchestrationHandlerException nullProcessedEventOrchestrationHandler)
            {
                throw CreateAndLogValidationException(nullProcessedEventOrchestrationHandler);
            }
            catch (ProcessedEventProcessingValidationException processedEventProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(processedEventProcessingValidationException);
            }
            catch (ProcessedEventProcessingDependencyValidationException
                processedEventProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(processedEventProcessingDependencyValidationException);
            }

            catch (ProcessedEventProcessingDependencyException processedEventProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(processedEventProcessingDependencyException);
            }
            catch (ProcessedEventProcessingServiceException processedEventProcessingServiceException)
            {
                throw CreateAndLogDependencyException(processedEventProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedProcessedEventOrchestrationServiceException =
                    new FailedProcessedEventOrchestrationServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedProcessedEventOrchestrationServiceException);
            }
        }

        private ProcessedEventOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventOrchestrationValidationException =
                new ProcessedEventOrchestrationValidationException(exception);

            return processedEventOrchestrationValidationException;
        }

        private ProcessedEventOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var processedEventOrchestrationDependencyValidationException =
                new ProcessedEventOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            throw processedEventOrchestrationDependencyValidationException;
        }

        private ProcessedEventOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var processedEventOrchestrationDependencyException =
                new ProcessedEventOrchestrationDependencyException(exception.InnerException as Xeption);

            throw processedEventOrchestrationDependencyException;
        }

        private ProcessedEventOrchestrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var processedEventOrchestrationServiceException =
                new ProcessedEventOrchestrationServiceException(exception);

            return processedEventOrchestrationServiceException;
        }
    }
}
