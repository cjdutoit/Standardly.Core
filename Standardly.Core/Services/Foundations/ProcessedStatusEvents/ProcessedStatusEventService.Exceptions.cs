// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Standardly.Core.Models.Events.ProcessedStatuses.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventService
    {
        private delegate void ReturningNothingFunction();

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (NullProcessedStatusEventHandler nullProcessedStatusEventHandler)
            {
                throw CreateAndLogValidationException(nullProcessedStatusEventHandler);
            }
        }

        private ProcessedStatusEventValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedStatusEventValidationException =
                new ProcessedStatusEventValidationException(exception);

            return processedStatusEventValidationException;
        }
    }
}
