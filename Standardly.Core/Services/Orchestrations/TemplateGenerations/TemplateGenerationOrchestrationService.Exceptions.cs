// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions;
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
        }

        private ProcessedEventOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventOrchestrationValidationException =
                new ProcessedEventOrchestrationValidationException(exception);

            return processedEventOrchestrationValidationException;
        }
    }
}
