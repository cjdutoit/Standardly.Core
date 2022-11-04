// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateOrchestrations.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationService
    {
        private delegate ValueTask<List<Template>> ReturningTemplateListFunction();

        private async ValueTask<List<Template>> TryCatchAsync(ReturningTemplateListFunction returningTemplateListFunction)
        {
            try
            {
                return await returningTemplateListFunction();
            }
            catch (FileValidationException fileValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileValidationException);
            }
            catch (FileDependencyValidationException fileDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileDependencyValidationException);
            }
        }

        private TemplateOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var templateOrchestrationDependencyValidationException =
                new TemplateOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            throw templateOrchestrationDependencyValidationException;
        }
    }
}
