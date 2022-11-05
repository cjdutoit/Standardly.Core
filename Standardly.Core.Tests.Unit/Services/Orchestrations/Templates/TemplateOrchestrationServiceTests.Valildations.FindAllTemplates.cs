// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldExcludeTemplatesThatDoesNotLoadCorrectlyTestsAsync()
        {
            // given
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
            List<Template> actualTemplates = await this.templateOrchestrationService.FindAllTemplatesAsync();

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
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
