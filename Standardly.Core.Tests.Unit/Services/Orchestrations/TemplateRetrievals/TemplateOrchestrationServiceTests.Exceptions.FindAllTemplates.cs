// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Orchestrations.TemplateRetrievals.Exceptions;
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
                new TemplateRetrievalOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(
                    templateFolderPath,
                    templateDefinitionFile))
                        .Throws(dependencyValidationException);

            // when
            Action findAllTemplatesAction = () =>
                templateRetrievalOrchestrationService.FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateRetrievalOrchestrationDependencyValidationException>(findAllTemplatesAction);

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
        public void ShouldThrowDependencyExceptionOnFindAllTemplatesIfDependencyErrorOccursAndLogIt(
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
                    .Throws(dependencyException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService
                    .FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationDependencyException actualException =
                Assert.Throws<TemplateRetrievalOrchestrationDependencyException>(findAllTemplatesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateRetrievalOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(broker =>
                broker.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile),
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

            var failedTemplateRetrievalOrchestrationServiceException =
                new FailedTemplateRetrievalOrchestrationServiceException(serviceException);

            var expectedTemplateRetrievalOrchestrationServiceException =
                new TemplateRetrievalOrchestrationServiceException(
                    failedTemplateRetrievalOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(broker =>
                broker.RetrieveListOfFilesAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService
                    .FindAllTemplates(templateFolderPath, templateDefinitionFile);

            TemplateRetrievalOrchestrationServiceException actualException =
                Assert.Throws<TemplateRetrievalOrchestrationServiceException>(findAllTemplatesAction);

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
