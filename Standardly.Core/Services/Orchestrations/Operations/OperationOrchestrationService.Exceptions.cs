// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Standardly.Core.Models.Orchestrations.Operations.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationService : IOperationOrchestrationService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentOperationOrchestrationException invalidArgumentOperationOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentOperationOrchestrationException);
            }
        }

        private OperationOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var operationOrchestrationValidationException =
                new OperationOrchestrationValidationException(exception);

            return operationOrchestrationValidationException;
        }
    }
}
