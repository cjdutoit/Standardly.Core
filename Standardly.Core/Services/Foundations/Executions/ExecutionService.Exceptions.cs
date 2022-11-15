// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Foundations.Executions.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Executions
{
    public partial class ExecutionService
    {
        private delegate string ReturningStringFunction();

        private string TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return returningStringFunction();
            }
            catch (InvalidArgumentExecutionException invalidArgumentExecutionException)
            {
                throw CreateAndLogValidationException(invalidArgumentExecutionException);
            }
            catch (Exception exception)
            {
                var failedExecutionServiceException =
                    new FailedExecutionServiceException(exception);

                throw CreateAndLogServiceException(failedExecutionServiceException);
            }
        }

        private ExecutionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var executionValidationException = new ExecutionValidationException(exception);
            this.loggingBroker.LogError(executionValidationException);

            return executionValidationException;
        }

        private ExecutionServiceException CreateAndLogServiceException(Xeption exception)
        {
            var executionServiceException = new ExecutionServiceException(exception);
            this.loggingBroker.LogError(executionServiceException);

            return executionServiceException;
        }
    }
}
