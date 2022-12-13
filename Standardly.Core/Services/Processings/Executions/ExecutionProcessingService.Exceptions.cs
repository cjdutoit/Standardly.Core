﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Foundations.Executions.Exceptions;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Executions
{
    public partial class ExecutionProcessingService
    {
        private delegate string ReturningStringFunction();

        private string TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return returningStringFunction();
            }
            catch (InvalidArgumentExecutionProcessingException invalidArgumentExecutionProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentExecutionProcessingException);
            }
            catch (ExecutionValidationException executionValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionValidationException);
            }
            catch (ExecutionDependencyValidationException executionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionDependencyValidationException);
            }
            catch (ExecutionDependencyException executionDependencyException)
            {
                throw CreateAndLogDependencyException(executionDependencyException);
            }
            catch (ExecutionServiceException executionServiceException)
            {
                throw CreateAndLogDependencyException(executionServiceException);
            }
            catch (Exception exception)
            {
                var failedExecutionProcessingServiceException =
                    new FailedExecutionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedExecutionProcessingServiceException);
            }
        }

        private ExecutionProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var executionProcessingValidationException =
                new ExecutionProcessingValidationException(exception);

            return executionProcessingValidationException;
        }

        private ExecutionProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var executionProcessingDependencyValidationException =
                new ExecutionProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            return executionProcessingDependencyValidationException;
        }

        private ExecutionProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var executionProcessingDependencyException =
                new ExecutionProcessingDependencyException(
                    exception.InnerException as Xeption);

            return executionProcessingDependencyException;
        }

        private ExecutionProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var executionProcessingServiceException = new
                ExecutionProcessingServiceException(exception);

            return executionProcessingServiceException;
        }
    }
}
