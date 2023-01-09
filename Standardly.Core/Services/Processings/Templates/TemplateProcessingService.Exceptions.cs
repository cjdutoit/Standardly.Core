// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Templates
{
    public partial class TemplateProcessingService
    {
        private delegate ValueTask<Template> ReturningTemplateFunction();

        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<Template> TryCatch(ReturningTemplateFunction returningTemplateFunction)
        {
            try
            {
                return await returningTemplateFunction();
            }
            catch (InvalidArgumentTemplateProcessingException invalidContentTemplateProcessingException)
            {
                throw CreateAndLogValidationException(invalidContentTemplateProcessingException);
            }
            catch (TemplateValidationException templateValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateValidationException);
            }
            catch (TemplateDependencyValidationException templateDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateDependencyValidationException);
            }
            catch (TemplateDependencyException templateDependencyException)
            {
                throw CreateAndLogDependencyException(templateDependencyException);
            }
            catch (TemplateServiceException templateServiceException)
            {
                throw CreateAndLogDependencyException(templateServiceException);
            }
            catch (Exception exception)
            {
                var failedTemplateProcessingServiceException =
                    new FailedTemplateProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTemplateProcessingServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentTemplateProcessingException invalidContentTemplateProcessingException)
            {
                throw CreateAndLogValidationException(invalidContentTemplateProcessingException);
            }
            catch (TemplateValidationException templateValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateValidationException);
            }
            catch (TemplateDependencyValidationException templateDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(templateDependencyValidationException);
            }
            catch (TemplateDependencyException templateDependencyException)
            {
                throw CreateAndLogDependencyException(templateDependencyException);
            }
            catch (TemplateServiceException templateServiceException)
            {
                throw CreateAndLogDependencyException(templateServiceException);
            }
            catch (Exception exception)
            {
                var failedTemplateProcessingServiceException =
                    new FailedTemplateProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedTemplateProcessingServiceException);
            }
        }

        private TemplateProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var templateProcessingValidationException =
                new TemplateProcessingValidationException(exception);

            return templateProcessingValidationException;
        }

        private TemplateProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var templateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            return templateProcessingDependencyValidationException;
        }

        private TemplateProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var templateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    exception.InnerException as Xeption);

            return templateProcessingDependencyException;
        }

        private TemplateProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var countryProcessingServiceException = new
                TemplateProcessingServiceException(exception);

            return countryProcessingServiceException;
        }
    }
}
