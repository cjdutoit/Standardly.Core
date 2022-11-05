// ---------------------------------------------------------------
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
            int randomNumber = GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Template randomTransformedTemplate = CreateRandomTemplate();
            List<Template> randomTransformedTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> outputTemplates = randomTemplates;
            string randomExecutionOutcome = GetRandomString();
            string randomTemplateString = GetRandomString();
            string randomTransformedTemplateString = GetRandomString();

            for (int i = 0; i < inputTemplates.Count; i++)
            {
                this.templateProcessingServiceMock.Setup(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(inputTemplates[i], randomReplacementDictionary))
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
                        });

                        // TODO:  Add code for Appends

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

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.WriteToFileAsync(file.Target, randomTransformedTemplateString),
                                    Times.Once);
                        });

                        // TODO:  Add code for Appends

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
