// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Orchestrations.TemplateRetrievals.Exceptions;
using Standardly.Core.Models.Services.Processings.Files.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnFindIfArgumentsIsInvalidAsync(string invalidText)
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
            ValueTask<List<Template>> findAllTemplatesAction =
                this.templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFileName);

            TemplateRetrievalOrchestrationValidationException actualTemplateRetrievalOrchestrationValidationException =
                await Assert.ThrowsAsync<TemplateRetrievalOrchestrationValidationException>(
                    findAllTemplatesAction.AsTask);

            // then
            actualTemplateRetrievalOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedTemplateRetrievalOrchestrationValidationException);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldExcludeTemplatesThatDoesNotLoadCorrectlyTestsAsync()
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
                fileService.RetrieveListOfFilesAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(expectedFileList);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(expectedTemplateString);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFileAsync(randomFileList[0]))
                    .ThrowsAsync(new FileProcessingDependencyException(new Xeption(randomFileList[0])));

            this.templateProcessingServiceMock.Setup(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString))
                .ReturnsAsync(outputTemplate);

            // when
            List<Template> actualTemplates = await this.templateRetrievalOrchestrationService
                .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFile);

            // then
            actualTemplates.Count.Should().Be(expectedFileList.Count - 1);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.RetrieveListOfFilesAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.ReadFromFileAsync(It.IsAny<string>()),
                    Times.Exactly(expectedFileList.Count));

            this.templateProcessingServiceMock.Verify(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString),
                    Times.Exactly(expectedFileList.Count - 1));

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
