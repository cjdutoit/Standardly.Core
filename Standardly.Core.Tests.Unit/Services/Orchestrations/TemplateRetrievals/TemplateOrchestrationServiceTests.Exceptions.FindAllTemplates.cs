// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionIfDependencyValidationErrorOccursAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var expectedDependencyValidationException =
                new TemplateGenerationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFiles(
                    templateFolderPath,
                    templateDefinitionFile))
                        .Throws(dependencyValidationException);

            // when
            Action findAllTemplatesAction = () =>
                templateRetrievalOrchestrationService.FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyValidationException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFiles(templateFolderPath, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnFindAllTemplatesIfDependencyErrorOccursAndLogIt(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someContent = GetRandomString();
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();

            var expectedTemplateGenerationOrchestrationDependencyException =
                new TemplateGenerationOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFiles(templateFolderPath, templateDefinitionFile))
                    .Throws(dependencyException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService
                    .FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateGenerationOrchestrationDependencyException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFiles(templateFolderPath, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnFindAllTemplatesIfServiceErrorOccurs()
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            var serviceException = new Exception();

            var failedTemplateGenerationOrchestrationServiceException =
                new FailedTemplateGenerationOrchestrationServiceException(serviceException);

            var expectedTemplateGenerationOrchestrationServiceException =
                new TemplateGenerationOrchestrationServiceException(
                    failedTemplateGenerationOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService
                    .FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateGenerationOrchestrationServiceException actualException =
                Assert.Throws<TemplateGenerationOrchestrationServiceException>(findAllTemplatesAction);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedTemplateGenerationOrchestrationServiceException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
