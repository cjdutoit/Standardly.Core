﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldGenerateCodeAsync()
        {
            // given
            int randomNumber = 1; // GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;
            List<Template> randomTransformedTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> outputTemplates = randomTransformedTemplates;
            string randomExecutionOutcome = GetRandomString();
            string randomTemplateString = GetRandomString();
            string randomTransformedTemplateString = GetRandomString();
            string randomFileContent = GetRandomString();
            string randomAppendedContent = GetRandomString();

            for (int i = 0; i < inputTemplates.Count; i++)
            {
                this.templateProcessingServiceMock.Setup(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(inputTemplates[i], inputDictionary))
                            .ReturnsAsync(outputTemplates[i]);

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    task.Actions.ForEach(action =>
                    {
                        action.Files.ForEach(file =>
                        {
                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.CheckIfFileExistsAsync(file.Target))
                                    .ReturnsAsync(true);

                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.ReadFromFileAsync(file.Template))
                                    .ReturnsAsync(randomTemplateString);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService
                                    .TransformStringAsync(randomTemplateString, It.IsAny<Dictionary<string, string>>()))
                                        .ReturnsAsync(randomTransformedTemplateString);
                        });

                        action.Appends.ForEach(append =>
                        {
                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.ReadFromFileAsync(append.Target))
                                    .ReturnsAsync(randomFileContent);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService.AppendContentAsync(
                                    randomFileContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist))
                                        .ReturnsAsync(randomAppendedContent);
                        });

                        this.executionProcessingServiceMock.Setup(executionProcessingService =>
                            executionProcessingService.RunAsync(action.Executions, action.ExecutionFolder))
                                .ReturnsAsync(randomExecutionOutcome);
                    });
                });
            }

            // when
            await templateOrchestrationService
                .GenerateCodeAsync(inputTemplates, randomReplacementDictionary);

            // then

            for (int i = 0; i < inputTemplates.Count; i++)
            {
                this.templateProcessingServiceMock.Verify(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(inputTemplates[i], randomReplacementDictionary),
                            Times.Exactly(2));

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    task.Actions.ForEach(action =>
                    {
                        action.Files.ForEach(file =>
                        {
                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.CheckIfFileExistsAsync(file.Target),
                                    Times.Exactly(2));

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.ReadFromFileAsync(file.Template),
                                    Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService
                                    .TransformStringAsync(randomTemplateString, It.IsAny<Dictionary<string, string>>()),
                                        Times.AtLeastOnce);

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.WriteToFileAsync(file.Target, randomTransformedTemplateString),
                                    Times.Once);
                        });

                        // TODO:  Add code for Appends

                        action.Appends.ForEach(append =>
                        {
                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.ReadFromFileAsync(append.Target),
                                    Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService.AppendContentAsync(
                                    randomFileContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist),
                                        Times.Once);

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService
                                    .WriteToFileAsync(append.Target, randomAppendedContent),
                                        Times.Once);
                        });

                        this.executionProcessingServiceMock.Verify(executionProcessingService =>
                            executionProcessingService.RunAsync(action.Executions, action.ExecutionFolder),
                                Times.Once);
                    });
                });
            }

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
