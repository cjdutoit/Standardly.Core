// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
    }
}
