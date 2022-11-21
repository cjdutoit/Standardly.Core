// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private delegate string ReturningStringFunction();
        private delegate Template ReturningTemplateFunction();
        private delegate void ReturningNothingFunction();

        private string TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return returningStringFunction();
            }
            catch (InvalidArgumentTemplateException invalidArgumentTemplateException)
            {
                throw CreateAndLogValidationException(invalidArgumentTemplateException);
            }
            catch (RegularExpressionTemplateException regularExpressionTemplateException)
            {
                throw CreateAndLogValidationException(regularExpressionTemplateException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidRegularExpressionTemplateException =
                    new InvalidRegularExpressionTemplateException(argumentNullException);

                throw CreateAndLogDependencyValidationException(invalidRegularExpressionTemplateException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidRegularExpressionTemplateException =
                    new InvalidRegularExpressionTemplateException(argumentOutOfRangeException);

                throw CreateAndLogDependencyValidationException(invalidRegularExpressionTemplateException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidRegularExpressionTemplateException =
                    new InvalidRegularExpressionTemplateException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidRegularExpressionTemplateException);
            }
            catch (Exception exception)
            {
                var failedTemplateServiceException =
                    new FailedTemplateServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedTemplateServiceException);
            }
        }

        private Template TryCatch(ReturningTemplateFunction returningTemplateFunction)
        {
            try
            {
                return returningTemplateFunction();
            }
            catch (InvalidArgumentTemplateException invalidArgumentTemplateException)
            {
                throw CreateAndLogValidationException(invalidArgumentTemplateException);
            }
            catch (InvalidTemplateException invalidTemplateException)
            {
                throw CreateAndLogValidationException(invalidTemplateException);
            }
            catch (Exception exception)
            {
                var failedTemplateServiceException =
                    new FailedTemplateServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedTemplateServiceException);
            }
        }

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (InvalidReplacementException invalidReplacementException)
            {
                throw CreateAndLogValidationException(invalidReplacementException);
            }
            catch (InvalidArgumentTemplateException invalidArgumentTemplateException)
            {
                throw CreateAndLogValidationException(invalidArgumentTemplateException);
            }
            catch (Exception exception)
            {
                var failedTemplateServiceException =
                    new FailedTemplateServiceException(exception.InnerException as Xeption);

                throw CreateAndLogServiceException(failedTemplateServiceException);
            }
        }

        private TemplateValidationException CreateAndLogValidationException(Xeption exception)
        {
            var templateValidationException = new TemplateValidationException(exception);
            this.loggingBroker.LogError(templateValidationException);

            return templateValidationException;
        }

        private TemplateDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var templateDependencyValidationException = new TemplateDependencyValidationException(exception);
            this.loggingBroker.LogError(templateDependencyValidationException);

            return templateDependencyValidationException;
        }

        private TemplateServiceException CreateAndLogServiceException(Exception exception)
        {
            var templateOrchestrationServiceException = new TemplateServiceException(exception);
            this.loggingBroker.LogError(templateOrchestrationServiceException);

            return templateOrchestrationServiceException;
        }
    }
}
