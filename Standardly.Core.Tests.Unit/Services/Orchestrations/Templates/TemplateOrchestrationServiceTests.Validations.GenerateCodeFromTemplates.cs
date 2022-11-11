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
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfArgumentsIsNullAsync()
        {
            // given
            List<Template> nullTemplateList = null;
            Dictionary<string, string> randomReplacementDictionary = null;

            var invalidArgumentTemplateOrchestrationException =
                new InvalidArgumentTemplateOrchestrationException();

            invalidArgumentTemplateOrchestrationException.AddData(
                key: "templates",
                values: "Templates is required");

            invalidArgumentTemplateOrchestrationException.AddData(
                key: "replacementDictionary",
                values: "Dictionary values is required");

            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                templateProcessingService
                    .TransformTemplateAsync(It.IsAny<Template>(), It.IsAny<Dictionary<string, string>>()))
                        .Throws(invalidArgumentTemplateOrchestrationException);

            var expectedTemplateOrchestrationValidationException =
                new TemplateOrchestrationValidationException(invalidArgumentTemplateOrchestrationException);

            // when
            ValueTask generateCodeTask =
               templateOrchestrationService.GenerateCodeAsync(nullTemplateList, randomReplacementDictionary);

            TemplateOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TemplateOrchestrationValidationException>(generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationValidationException);

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemoveTemplatesOnGenerateCodeIfNotRequiredAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, false);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;
            List<Template> randomTransformedTemplates = GetRandomTemplateList(randomNumber, false);
            List<Template> outputTemplates = randomTransformedTemplates;
            string randomExecutionOutcome = GetRandomString();
            string randomTemplateString = GetRandomString();
            string randomTransformedTemplateString = GetRandomString();
            List<string> targets = new List<string>();

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

                            targets.Add(file.Target);
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
                            Times.Once);
            }

            targets.ForEach(target =>
            {
                this.fileProcessingServiceMock.Verify(fileProcessingService =>
                    fileProcessingService.CheckIfFileExistsAsync(target),
                        Times.Once);
            });

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
