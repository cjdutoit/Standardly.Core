// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions;
using Standardly.Core.Models.Services.Processings.Executions.Exceptions;
using Standardly.Core.Models.Services.Processings.Files.Exceptions;
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationService
    {
        private delegate ValueTask<List<Template>> ReturningTemplateListFunction();

        private async ValueTask<List<Template>> TryCatch(ReturningTemplateListFunction returningTemplateListFunction)
        {
            try
            {
                return await returningTemplateListFunction();
            }
            catch (InvalidArgumentTemplateRetrievalOrchestrationException
                invalidArgumentTemplateRetrievalOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentTemplateRetrievalOrchestrationException);
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
                    new FailedTemplateRetrievalOrchestrationServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedTemplateOrchestrationServiceException);
            }
        }

        private TemplateRetrievalOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var templateOrchestrationValidationException =
                new TemplateRetrievalOrchestrationValidationException(exception);

            return templateOrchestrationValidationException;
        }

        private TemplateRetrievalOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var templateOrchestrationDependencyValidationException =
                new TemplateRetrievalOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            throw templateOrchestrationDependencyValidationException;
        }

        private TemplateRetrievalOrchestrationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var templateOrchestrationDependencyException =
                new TemplateRetrievalOrchestrationDependencyException(exception.InnerException as Xeption);

            throw templateOrchestrationDependencyException;
        }

        private TemplateRetrievalOrchestrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var templateOrchestrationServiceException = new TemplateRetrievalOrchestrationServiceException(exception);

            return templateOrchestrationServiceException;
        }
    }
}
