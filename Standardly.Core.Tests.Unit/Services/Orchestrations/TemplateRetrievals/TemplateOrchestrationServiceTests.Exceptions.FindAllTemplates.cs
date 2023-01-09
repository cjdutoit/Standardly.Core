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
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionIfDependencyValidationErrorOccursAsync(
            Exception dependencyValidationException)
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var expectedDependencyValidationException =
                new TemplateRetrievalOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(
                    templateFolderPath,
                    templateDefinitionFile))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Template>> findAllTemplatesTask =
                templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateRetrievalOrchestrationDependencyValidationException>(
                    findAllTemplatesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnFindAllTemplatesIfDependencyErrorOccursAsync(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someContent = GetRandomString();
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();

            var expectedTemplateRetrievalOrchestrationDependencyException =
                new TemplateRetrievalOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Template>> findAllTemplatesTask =
                this.templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TemplateRetrievalOrchestrationDependencyException>(
                    findAllTemplatesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateRetrievalOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnFindAllTemplatesIfServiceErrorOccursAsync()
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            var serviceException = new Exception();

            var failedTemplateRetrievalOrchestrationServiceException =
                new FailedTemplateRetrievalOrchestrationServiceException(serviceException);

            var expectedTemplateRetrievalOrchestrationServiceException =
                new TemplateRetrievalOrchestrationServiceException(
                    failedTemplateRetrievalOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFilesAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Template>> findAllTemplatesTask =
                this.templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TemplateRetrievalOrchestrationServiceException>(
                    findAllTemplatesTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedTemplateRetrievalOrchestrationServiceException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
