// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;
using Standardly.Core.Models.Processings.ProcessedEvents.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingService : IProcessedEventProcessingService
    {
        private delegate void ReturningNothingFunction();

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
        }

        private ProcessedEventProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventProcessingValidationException =
                new ProcessedEventProcessingValidationException(exception);

            return processedEventProcessingValidationException;
        }

        private ProcessedEventProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var processedEventProcessingDependencyValidationException =
                new ProcessedEventProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            return processedEventProcessingDependencyValidationException;
        }
    }
}
