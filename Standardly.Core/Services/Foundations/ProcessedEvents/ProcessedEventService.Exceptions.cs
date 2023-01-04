// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
            catch (NullProcessedEventHandler nullProcessedEventHandler)
            {
                throw CreateAndLogValidationException(nullProcessedEventHandler);
            }
        }


        private ProcessedEventValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedEventValidationException =
                new ProcessedEventValidationException(exception);

            return processedEventValidationException;
        }
    }
}
