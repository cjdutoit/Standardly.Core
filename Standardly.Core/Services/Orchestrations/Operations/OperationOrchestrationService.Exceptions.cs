// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Standardly.Core.Models.Orchestrations.Operations.Exceptions;
using Standardly.Core.Models.Processings.Executions.Exceptions;
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
            catch (ExecutionProcessingValidationException executionProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionProcessingValidationException);
            }
            catch (ExecutionProcessingDependencyValidationException executionProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionProcessingDependencyValidationException);
            }
            catch (ExecutionProcessingDependencyException executionDependencyException)
            {
                throw CreateAndLogDependencyException(executionDependencyException);
            }
            catch (ExecutionProcessingServiceException executionServiceException)
            {
                throw CreateAndLogDependencyException(executionServiceException);
            }
        }

        private OperationOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var operationOrchestrationValidationException =
                new OperationOrchestrationValidationException(exception);

            return operationOrchestrationValidationException;
        }

        private OperationOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var operationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    exception.InnerException as Xeption);

            return operationOrchestrationDependencyValidationException;
        }

        private OperationOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var operationOrchestrationDependencyException =
                new OperationOrchestrationDependencyException(
                    exception.InnerException as Xeption);

            return operationOrchestrationDependencyException;
        }

    }
}
