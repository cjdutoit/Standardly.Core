// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<Template> ReturningTemplateFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
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

        private async ValueTask<Template> TryCatch(ReturningTemplateFunction returningTemplateFunction)
        {
            try
            {
                return await returningTemplateFunction();
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

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidReplacementTemplateException invalidReplacementException)
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

            return templateValidationException;
        }

        private TemplateDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var templateDependencyValidationException = new TemplateDependencyValidationException(exception);

            return templateDependencyValidationException;
        }

        private TemplateServiceException CreateAndLogServiceException(Exception exception)
        {
            var templateServiceException = new TemplateServiceException(exception);

            return templateServiceException;
        }
    }
}
