// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateRetrievals.Exceptions;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowValidationExceptionOnFindIfArgumentsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            string templateFolderPath = invalidText;
            string templateDefinitionFileName = invalidText;

            var invalidArgumentTemplateRetrievalOrchestrationException = new
                InvalidArgumentTemplateRetrievalOrchestrationException();

            invalidArgumentTemplateRetrievalOrchestrationException.AddData(
                key: nameof(templateFolderPath),
                values: "Text is required");

            invalidArgumentTemplateRetrievalOrchestrationException.AddData(
                key: nameof(templateDefinitionFileName),
                values: "Text is required");

            var expectedTemplateRetrievalOrchestrationValidationException =
                new TemplateRetrievalOrchestrationValidationException(
                    invalidArgumentTemplateRetrievalOrchestrationException);

            // when
            System.Action findAllTemplatesAction = () =>
                this.templateRetrievalOrchestrationService
                    .FindAllTemplates(templateFolderPath, templateDefinitionFileName);

            TemplateRetrievalOrchestrationValidationException actualTemplateRetrievalOrchestrationValidationException =
                Assert.Throws<TemplateRetrievalOrchestrationValidationException>(findAllTemplatesAction);

            // then
            actualTemplateRetrievalOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedTemplateRetrievalOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateRetrievalOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldExcludeTemplatesThatDoesNotLoadCorrectlyTests()
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            int itemsToGenerate = GetRandomNumber();
            List<string> randomFileList = CreateListOfStrings();
            List<string> expectedFileList = randomFileList;
            string randomTemplateString = GetRandomString();
            string inputTemplateString = randomTemplateString;
            string expectedTemplateString = inputTemplateString;
            string rawTemplateString = expectedTemplateString;
            Template randomTemplate = CreateRandomTemplate(itemsToGenerate);
            Template outputTemplate = randomTemplate;

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(expectedFileList);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFile(It.IsAny<string>()))
                    .Returns(expectedTemplateString);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFile(randomFileList[0]))
                    .Throws(new FileProcessingDependencyException(new Xeption(randomFileList[0])));

            this.templateProcessingServiceMock.Setup(templateService =>
                templateService.ConvertStringToTemplate(rawTemplateString))
                .Returns(outputTemplate);

            // when
            List<Template> actualTemplates = this.templateRetrievalOrchestrationService
                .FindAllTemplates(templateFolderPath, templateDefinitionFile);

            // then
            actualTemplates.Count.Should().Be(expectedFileList.Count - 1);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.RetrieveListOfFiles(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.ReadFromFile(It.IsAny<string>()),
                    Times.Exactly(expectedFileList.Count));

            this.templateProcessingServiceMock.Verify(templateService =>
                templateService.ConvertStringToTemplate(rawTemplateString),
                    Times.Exactly(expectedFileList.Count - 1));

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
