// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateOrchestrations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(FindAllTemplateOrchestrationTemplatesDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionIfDependencyValidationErrorOccursAndLogItAsync(
            Exception dependencyValidationException)
        {
            // given
            string templatefolder = this.templateConfigMock.Object.TemplateFolder;
            string templateDefinitionFile = this.templateConfigMock.Object.TemplateDefinitionFile;
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var expectedDependencyValidationException =
                new TemplateOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(
                    templatefolder,
                    templateDefinitionFile))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Template>> findAllTemplatesTask =
                templateOrchestrationService.FindAllTemplatesAsync();

            TemplateOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateOrchestrationDependencyValidationException>(
                    findAllTemplatesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFilesAsync(templatefolder, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
