// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Executions
{
    public partial class ExecutionProcessingService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentExecutionProcessingException invalidArgumentExecutionProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentExecutionProcessingException);
            }
        }

        private ExecutionProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var executionProcessingValidationException =
                new ExecutionProcessingValidationException(exception);

            this.loggingBroker.LogError(executionProcessingValidationException);

            return executionProcessingValidationException;
        }
    }
}
