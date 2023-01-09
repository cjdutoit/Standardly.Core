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
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationServiceTests
    {

        [Fact]
        public async Task ShouldFindAllTemplatesAsync()
        {
            // given
            string templateFolderPath = GetRandomString();
            string templateDefinitionFile = GetRandomString();
            List<string> randomFileList = GetRandomStringList();
            List<string> expectedFileList = randomFileList;
            string randomTemplateString = GetRandomString();
            string inputTemplateString = randomTemplateString;
            string expectedTemplateString = inputTemplateString;
            string rawTemplateString = expectedTemplateString;
            Template randomTemplate = CreateRandomTemplate(GetRandomNumber(), true);
            Template outputTemplate = randomTemplate;


            fileProcessingServiceMock.Setup(fileService =>
                fileService.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile))
                    .ReturnsAsync(expectedFileList);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(expectedTemplateString);

            this.templateProcessingServiceMock.Setup(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString))
                    .ReturnsAsync(outputTemplate);

            // when
            List<Template> actualTemplates = await this.templateRetrievalOrchestrationService
                .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFile);

            // then
            actualTemplates.Count.Should().Be(expectedFileList.Count);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.RetrieveListOfFilesAsync(templateFolderPath, templateDefinitionFile),
                        Times.Once);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.ReadFromFileAsync(It.IsAny<string>()),
                    Times.Exactly(expectedFileList.Count));

            this.templateProcessingServiceMock.Verify(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString),
                    Times.Exactly(expectedFileList.Count));

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
