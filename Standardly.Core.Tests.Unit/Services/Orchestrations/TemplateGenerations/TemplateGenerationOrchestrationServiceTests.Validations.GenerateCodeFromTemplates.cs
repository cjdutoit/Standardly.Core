// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public void ShouldThrowValidationExceptionWhenIfTemplateIsNullAndLogItAsync()
        {
            // given
            TemplateGenerationInfo nullTemplateGenerationInfo = null;

            var nullTemplateGenerationOrchestrationException =
                new NullTemplateGenerationOrchestrationException();

            var expectedTemplateGenerationOrchestrationValidationException =
                new TemplateGenerationOrchestrationValidationException(nullTemplateGenerationOrchestrationException);

            // when
            Action generateCodeAction = () =>
               templateGenerationOrchestrationService.GenerateCode(nullTemplateGenerationInfo);

            TemplateGenerationOrchestrationValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationValidationException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationValidationException);

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowValidationExceptionIfArgumentsIsNull()
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
            Action generateCodeAction = () =>
               templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);

            TemplateGenerationOrchestrationValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationValidationException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationValidationException);

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRemoveTemplatesOnGenerateCodeIfNotRequired()
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
                        .TransformTemplate(
                            templateGenerationInfo.Templates[i],
                            templateGenerationInfo.ReplacementDictionary))
                                .Returns(outputTemplates[i]);

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    task.Actions.ForEach(action =>
                    {
                        action.Files.ForEach(file =>
                        {
                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.CheckIfFileExists(file.Target))
                                    .Returns(true);

                            targets.Add(file.Target);
                        });

                        this.executionProcessingServiceMock.Setup(executionProcessingService =>
                            executionProcessingService.Run(action.Executions, action.ExecutionFolder))
                                .Returns(randomExecutionOutcome);
                    });
                });
            }

            // when
            templateGenerationOrchestrationService
                .GenerateCode(templateGenerationInfo);

            // then
            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Verify(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplate(
                            templateGenerationInfo.Templates[i],
                            templateGenerationInfo.ReplacementDictionary),
                                Times.Once);
            }

            targets.ForEach(target =>
            {
                this.fileProcessingServiceMock.Verify(fileProcessingService =>
                    fileProcessingService.CheckIfFileExists(target),
                        Times.Once);
            });

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowValidationExceptionIfAllTagsNotReplacedWithinAppend()
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
            this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled = true;
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
                        .TransformTemplate(
                            templateGenerationInfo.Templates[i],
                            templateGenerationInfo.ReplacementDictionary))
                                .Returns(outputTemplates[i]);

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    this.templateProcessingServiceMock.Setup(templateProcessingService =>
                        templateProcessingService
                            .TransformString(task.BranchName, templateGenerationInfo.ReplacementDictionary));

                    task.Actions.ForEach(action =>
                    {
                        action.Files.ForEach(file =>
                        {
                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.CheckIfFileExists(file.Target))
                                    .Returns(true);

                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.ReadFromFile(file.Template))
                                    .Returns(randomTemplateString);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService
                                    .TransformString(randomTemplateString, It.IsAny<Dictionary<string, string>>()))
                                        .Returns(randomTransformedTemplateString);
                        });

                        action.Appends.ForEach(append =>
                        {
                            this.fileProcessingServiceMock.Setup(fileProcessingService =>
                                fileProcessingService.ReadFromFile(append.Target))
                                    .Returns(randomFileContent);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService.AppendContent(
                                    randomFileContent,
                                    append.DoesNotContainContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist))
                                        .Returns(randomAppendedContent);

                            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                                templateProcessingService
                                    .TransformString(randomAppendedContent, randomReplacementDictionary))
                                        .Throws(templateProcessingDependencyValidationException);
                        });

                        this.executionProcessingServiceMock.Setup(executionProcessingService =>
                            executionProcessingService.Run(action.Executions, action.ExecutionFolder))
                                .Returns(randomExecutionOutcome);
                    });
                });
            }

            // when
            Action GenerateCodeAction = () =>
                templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyValidationException>(GenerateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedException))),
                        Times.Once);

            for (int i = 0; i < templateGenerationInfo.Templates.Count; i++)
            {
                this.templateProcessingServiceMock.Verify(templateProcessingService =>
                    templateProcessingService
                        .TransformTemplate(
                            templateGenerationInfo.Templates[i],
                            templateGenerationInfo.ReplacementDictionary),
                                Times.Exactly(2));

                outputTemplates[i].Tasks.ForEach(task =>
                {
                    this.templateProcessingServiceMock.Verify(templateProcessingService =>
                        templateProcessingService
                            .TransformString(
                                task.BranchName,
                                templateGenerationInfo.ReplacementDictionary),
                                    Times.AtLeastOnce);

                    task.Actions.ForEach(action =>
                    {
                        action.Files.ForEach(file =>
                        {
                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.CheckIfFileExists(file.Target),
                                    Times.Exactly(2));

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.ReadFromFile(file.Template),
                                    Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService
                                    .TransformString(randomTemplateString, It.IsAny<Dictionary<string, string>>()),
                                        Times.AtLeastOnce);

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.WriteToFile(file.Target, randomTransformedTemplateString),
                                    Times.Once);
                        });

                        action.Appends.ForEach(append =>
                        {
                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService.ReadFromFile(append.Target),
                                    Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService.AppendContent(
                                    randomFileContent,
                                    append.DoesNotContainContent,
                                    append.RegexToMatchForAppend,
                                    append.ContentToAppend,
                                    append.AppendToBeginning,
                                    append.AppendEvenIfContentAlreadyExist),
                                        Times.Once);

                            this.templateProcessingServiceMock.Verify(templateProcessingService =>
                                templateProcessingService
                                    .TransformString(randomAppendedContent, randomReplacementDictionary),
                                        Times.Once);

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService
                                    .WriteToFile(append.Target, randomAppendedContent),
                                        Times.Never);
                        });

                        this.executionProcessingServiceMock.Verify(executionProcessingService =>
                            executionProcessingService.Run(action.Executions, action.ExecutionFolder),
                                Times.Never);
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
