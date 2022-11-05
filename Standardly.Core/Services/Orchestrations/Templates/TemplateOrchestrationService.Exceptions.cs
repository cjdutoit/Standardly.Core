// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationService
    {
        private delegate ValueTask<List<Template>> ReturningTemplateListFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<Template>> TryCatchAsync(ReturningTemplateListFunction returningTemplateListFunction)
        {
            try
            {
                return await returningTemplateListFunction();
            }
            catch (FileProcessingValidationException fileServiceValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileServiceValidationException);
            }
            catch (FileProcessingDependencyValidationException fileServiceDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileServiceDependencyValidationException);
            }
            catch (ExecutionProcessingValidationException executionValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionValidationException);
            }
            catch (ExecutionProcessingDependencyValidationException executionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionDependencyValidationException);
            }
            catch (TemplateProcessingValidationException templateValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateValidationException);
            }
            catch (TemplateProcessingDependencyValidationException templateDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateDependencyValidationException);
            }
            catch (FileProcessingServiceException fileServiceException)
            {
                throw CreateAndLogDependencyException(fileServiceException);
            }
            catch (FileProcessingDependencyException fileServiceDependencyException)
            {
                throw CreateAndLogDependencyException(fileServiceDependencyException);
            }
            catch (TemplateProcessingServiceException templateServiceException)
            {
                throw CreateAndLogDependencyException(templateServiceException);
            }
            catch (TemplateProcessingDependencyException templateDependencyException)
            {
                throw CreateAndLogDependencyException(templateDependencyException);
            }
            catch (ExecutionProcessingServiceException executionServiceException)
            {
                throw CreateAndLogDependencyException(executionServiceException);
            }
            catch (ExecutionProcessingDependencyException executionDependencyException)
            {
                throw CreateAndLogDependencyException(executionDependencyException);
            }
            catch (Exception exception)
            {
                var failedTemplateOrchestrationServiceException =
                    new FailedTemplateOrchestrationServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedTemplateOrchestrationServiceException);
            }
        }

        private async ValueTask TryCatchAsync(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentTemplateOrchestrationException invalidArgumentTemplateOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentTemplateOrchestrationException);
            }
            catch (FileProcessingValidationException fileServiceValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileServiceValidationException);
            }
            catch (FileProcessingDependencyValidationException fileServiceDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileServiceDependencyValidationException);
            }
            catch (ExecutionProcessingValidationException executionValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionValidationException);
            }
            catch (ExecutionProcessingDependencyValidationException executionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(executionDependencyValidationException);
            }
            catch (TemplateProcessingValidationException templateValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateValidationException);
            }
            catch (TemplateProcessingDependencyValidationException templateDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateDependencyValidationException);
            }
            catch (FileProcessingServiceException fileServiceException)
            {
                throw CreateAndLogDependencyException(fileServiceException);
            }
            catch (FileProcessingDependencyException fileServiceDependencyException)
            {
                throw CreateAndLogDependencyException(fileServiceDependencyException);
            }
            catch (TemplateProcessingServiceException templateServiceException)
            {
                throw CreateAndLogDependencyException(templateServiceException);
            }
            catch (TemplateProcessingDependencyException templateDependencyException)
            {
                throw CreateAndLogDependencyException(templateDependencyException);
            }
            catch (ExecutionProcessingServiceException executionServiceException)
            {
                throw CreateAndLogDependencyException(executionServiceException);
            }
            catch (ExecutionProcessingDependencyException executionDependencyException)
            {
                throw CreateAndLogDependencyException(executionDependencyException);
            }
        }

        private TemplateOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var templateOrchestrationValidationException =
                new TemplateOrchestrationValidationException(exception);

            this.loggingBroker.LogError(templateOrchestrationValidationException);

            return templateOrchestrationValidationException;
        }

        private TemplateOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var templateOrchestrationDependencyValidationException =
                new TemplateOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            throw templateOrchestrationDependencyValidationException;
        }

        private TemplateOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var templateOrchestrationDependencyException =
                new TemplateOrchestrationDependencyException(exception.InnerException as Xeption);

            throw templateOrchestrationDependencyException;
        }

        private TemplateOrchestrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var templateOrchestrationServiceException = new TemplateOrchestrationServiceException(exception);

            return templateOrchestrationServiceException;
        }
    }
}
