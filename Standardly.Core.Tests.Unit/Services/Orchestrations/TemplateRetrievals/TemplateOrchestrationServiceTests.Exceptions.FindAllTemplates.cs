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
            string templatefolder = this.templateConfigMock.Object.TemplateFolderPath;
            string templateDefinitionFile = this.templateConfigMock.Object.TemplateDefinitionFileName;
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var expectedDependencyValidationException =
                new TemplateGenerationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFiles(
                    templatefolder,
                    templateDefinitionFile))
                        .Throws(dependencyValidationException);

            // when
            Action findAllTemplatesAction = () =>
                templateRetrievalOrchestrationService.FindAllTemplates();

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyValidationException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFiles(templatefolder, templateDefinitionFile),
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
            string templatefolder = this.templateConfigMock.Object.TemplateFolderPath;
            string templateDefinitionFile = this.templateConfigMock.Object.TemplateDefinitionFileName;

            var expectedTemplateGenerationOrchestrationDependencyException =
                new TemplateGenerationOrchestrationDependencyException(dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFiles(templatefolder, templateDefinitionFile))
                    .Throws(dependencyException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService.FindAllTemplates();

            TemplateGenerationOrchestrationDependencyException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFiles(templatefolder, templateDefinitionFile),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnFindAllTemplatesIfServiceErrorOccurs()
        {
            // given
            var serviceException = new Exception();

            var failedTemplateGenerationOrchestrationServiceException =
                new FailedTemplateGenerationOrchestrationServiceException(serviceException);

            var expectedTemplateGenerationOrchestrationServiceException =
                new TemplateGenerationOrchestrationServiceException(failedTemplateGenerationOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService.FindAllTemplates();

            TemplateGenerationOrchestrationServiceException actualException =
                Assert.Throws<TemplateGenerationOrchestrationServiceException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationServiceException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
