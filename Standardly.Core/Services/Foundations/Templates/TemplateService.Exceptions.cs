// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatchAsync(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
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

        private TemplateServiceException CreateAndLogServiceException(Exception exception)
        {
            var templateOrchestrationServiceException = new TemplateServiceException(exception);

            return templateOrchestrationServiceException;
        }
    }
}
