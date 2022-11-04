// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Templates
{
    public partial class TemplateProcessingService
    {
        private delegate ValueTask<Template> ReturningTemplateFunction();

        private async ValueTask<Template> TryCatchAsync(ReturningTemplateFunction returningTemplateFunction)
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
        }

        private TemplateProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var templateProcessingValidationException =
                new TemplateProcessingValidationException(exception);

            this.loggingBroker.LogError(templateProcessingValidationException);

            return templateProcessingValidationException;
        }

        private TemplateProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var templateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(templateProcessingDependencyValidationException);

            return templateProcessingDependencyValidationException;
        }

        private TemplateProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var templateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(templateProcessingDependencyException);

            return templateProcessingDependencyException;
        }
    }
}
