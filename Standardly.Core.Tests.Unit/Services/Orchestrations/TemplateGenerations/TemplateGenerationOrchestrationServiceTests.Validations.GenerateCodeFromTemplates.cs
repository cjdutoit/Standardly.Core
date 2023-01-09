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
using Standardly.Core.Models.Foundations.Templates.EntityModels;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Orchestrations;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Standardly.Core.Models.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionWhenIfTemplateIsNullAsync()
        {
            // given
            TemplateGenerationInfo nullTemplateGenerationInfo = null;

            var nullTemplateGenerationOrchestrationException =
                new NullTemplateGenerationOrchestrationException();

            var expectedTemplateGenerationOrchestrationValidationException =
                new TemplateGenerationOrchestrationValidationException(
                    nullTemplateGenerationOrchestrationException);

            // when
            ValueTask generateCodeTask =
               templateGenerationOrchestrationService.GenerateCodeAsync(nullTemplateGenerationInfo);

            TemplateGenerationOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationValidationException>(
                    generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationValidationException);

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfArgumentsIsNullAsync()
        {
            // given
            List<Template> nullTemplateList = null;
            Dictionary<string, string> nullReplacementDictionary = null;
            List<EntityModel> entityModelDefinition = null;

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = nullTemplateList,
                    ReplacementDictionary = nullReplacementDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            var invalidArgumentTemplateGenerationOrchestrationException =
                new InvalidArgumentTemplateGenerationOrchestrationException();

            invalidArgumentTemplateGenerationOrchestrationException.AddData(
                key: "Templates",
                values: "Templates is required");

            invalidArgumentTemplateGenerationOrchestrationException.AddData(
                key: "ReplacementDictionary",
                values: "Dictionary is required");

            invalidArgumentTemplateGenerationOrchestrationException.AddData(
                key: "EntityModelDefinition",
                values: "Dictionary is required");

            var expectedTemplateGenerationOrchestrationValidationException =
                new TemplateGenerationOrchestrationValidationException(
                    invalidArgumentTemplateGenerationOrchestrationException);

            // when
            ValueTask generateCodeTask =
               templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);

            TemplateGenerationOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationValidationException>(
                    generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationValidationException);

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
            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = inputTemplates,
                    ReplacementDictionary = inputDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Setup(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(
                            templateGenerationInfo.Templates[i]))
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
            await templateGenerationOrchestrationService
                .GenerateCodeAsync(templateGenerationInfo);

            // then
            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Verify(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(
                            templateGenerationInfo.Templates[i]),
                                Times.Once);
            }

            targets.ForEach(target =>
            {
                this.fileProcessingServiceMock.Verify(fileProcessingService =>
                    fileProcessingService.CheckIfFileExistsAsync(target),
                        Times.Once);
            });

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfAllTagsNotReplacedWithinAppendAsync()
        {
            // given
            var invalidReplacementException = new InvalidReplacementTemplateException();

            var templateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(invalidReplacementException);

            var expectedException =
                new TemplateGenerationOrchestrationDependencyValidationException(
                    templateProcessingDependencyValidationException.InnerException as Xeption);

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
            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = inputTemplates,
                    ReplacementDictionary = inputDictionary,
                    EntityModelDefinition = entityModelDefinition,
                    ScriptExecutionIsEnabled = true
                };

            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Setup(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(
                            templateGenerationInfo.Templates[i]))
                                .ReturnsAsync(outputTemplates[i]);

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    this.templateProcessingServiceMock.Setup(templateProcessingService =>
                        templateProcessingService
                            .TransformStringAsync(task.BranchName, templateGenerationInfo.ReplacementDictionary));

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
                                    append.DoesNotContainContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist))
                                        .ReturnsAsync(randomAppendedContent);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService
                                    .TransformStringAsync(randomAppendedContent, randomReplacementDictionary))
                                        .ThrowsAsync(templateProcessingDependencyValidationException);
                        });

                        this.executionProcessingServiceMock.Setup(executionProcessingService =>
                            executionProcessingService.RunAsync(action.Executions, action.ExecutionFolder))
                                .ReturnsAsync(randomExecutionOutcome);
                    });
                });
            }

            // when
            ValueTask GenerateCodeAction =
                templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationDependencyValidationException>(
                    GenerateCodeAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Verify(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplateAsync(
                            templateGenerationInfo.Templates[i]),
                                Times.Exactly(2));

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    this.templateProcessingServiceMock.Verify(templateProcessingService =>
                        templateProcessingService
                            .TransformStringAsync(
                                task.BranchName,
                                templateGenerationInfo.ReplacementDictionary),
                                    Times.AtLeastOnce);

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

                        action.Appends.ForEach(append =>
                        {
                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.ReadFromFileAsync(append.Target),
                                    Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService.AppendContentAsync(
                                    randomFileContent,
                                    append.DoesNotContainContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist),
                                        Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService
                                    .TransformStringAsync(randomAppendedContent, randomReplacementDictionary),
                                        Times.Once);

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService
                                    .WriteToFileAsync(append.Target, randomAppendedContent),
                                        Times.Never);
                        });

                        this.executionProcessingServiceMock.Verify(executionProcessingService =>
                            executionProcessingService.RunAsync(action.Executions, action.ExecutionFolder),
                                Times.Never);
                    });
                });
            }

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
