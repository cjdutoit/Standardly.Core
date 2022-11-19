// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.EntityModels;
using Standardly.Core.Models.Orchestrations;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationServiceTests
    {
        [Fact]
        public void ShouldGenerateCodeIncludingScriptExecution()
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

                        // TODO:  Add code for Appends

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

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService
                                    .WriteToFile(append.Target, randomAppendedContent),
                                        Times.Once);
                        });

                        this.executionProcessingServiceMock.Verify(executionProcessingService =>
                            executionProcessingService.Run(action.Executions, action.ExecutionFolder),
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

        [Fact]
        public void ShouldGenerateCodeWithOutScriptExecution()
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
            this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled = false;
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
                            .TransformString(
                                task.BranchName,
                                templateGenerationInfo.ReplacementDictionary));

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

                        // TODO:  Add code for Appends

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

                            this.fileProcessingServiceMock.Verify(fileProcessingService =>
                                fileProcessingService
                                    .WriteToFile(append.Target, randomAppendedContent),
                                        Times.Once);
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
