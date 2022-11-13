﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {

        [Fact]
        public async Task ShouldFindAllTemplatesAsync()
        {
            // given
            string templatefolder = this.templateConfigMock.Object.TemplateFolderPath;
            string templateDefinitionFile = this.templateConfigMock.Object.TemplateDefinitionFileName;
            List<string> randomFileList = GetRandomStringList();
            List<string> expectedFileList = randomFileList;
            string randomTemplateString = GetRandomString();
            string inputTemplateString = randomTemplateString;
            string expectedTemplateString = inputTemplateString;
            string rawTemplateString = expectedTemplateString;
            Template randomTemplate = CreateRandomTemplate(GetRandomNumber(), true);
            Template outputTemplate = randomTemplate;


            fileProcessingServiceMock.Setup(fileService =>
                fileService.RetrieveListOfFiles(templatefolder, templateDefinitionFile))
                    .Returns(expectedFileList);

            this.fileProcessingServiceMock.Setup(fileService =>
                fileService.ReadFromFile(It.IsAny<string>()))
                    .Returns(expectedTemplateString);

            this.templateProcessingServiceMock.Setup(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString))
                    .ReturnsAsync(outputTemplate);

            // when
            List<Template> actualTemplates = await this.templateOrchestrationService.FindAllTemplatesAsync();

            // then
            actualTemplates.Count.Should().Be(expectedFileList.Count);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.RetrieveListOfFiles(templatefolder, templateDefinitionFile),
                        Times.Once);

            this.fileProcessingServiceMock.Verify(fileService =>
                fileService.ReadFromFile(It.IsAny<string>()),
                    Times.Exactly(expectedFileList.Count));

            this.templateProcessingServiceMock.Verify(templateService =>
                templateService.ConvertStringToTemplateAsync(rawTemplateString),
                    Times.Exactly(expectedFileList.Count));

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
