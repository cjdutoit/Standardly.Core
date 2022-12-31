// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
            catch (Exception exception)
            {
                var failedProcessedStatusEventServiceException =
                    new FailedProcessedStatusEventServiceException(exception);

                throw CreateAndLogServiceException(failedProcessedStatusEventServiceException);
            }
        }

        private ProcessedStatusEventValidationException CreateAndLogValidationException(Xeption exception)
        {
            var processedStatusEventValidationException =
                new ProcessedStatusEventValidationException(exception);

            return processedStatusEventValidationException;
        }

        private ProcessedStatusEventServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var processedStatusEventServiceException = new ProcessedStatusEventServiceException(exception);

            return processedStatusEventServiceException;
        }
    }
}
