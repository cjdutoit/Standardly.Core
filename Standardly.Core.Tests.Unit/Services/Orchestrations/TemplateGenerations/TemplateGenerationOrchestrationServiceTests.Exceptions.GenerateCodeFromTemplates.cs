﻿// ---------------------------------------------------------------
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
using Standardly.Core.Models.Orchestrations;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnGenerateCodeFromTemplateAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            int randomNumber = GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;
            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = inputTemplates,
                    ReplacementDictionary = inputDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            var expectedDependencyValidationException =
                new TemplateGenerationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(dependencyValidationException);

            // when
            Action generateCodeAction = () =>
                this.templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyValidationException>(
                    generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnGenerateCodeFromTemplateAndLogIt(
            Exception dependencyException)
        {
            // given
            int randomNumber = GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;
            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = inputTemplates,
                    ReplacementDictionary = inputDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            var expectedTemplateOrchestrationDependencyException =
                new TemplateGenerationOrchestrationDependencyException(dependencyException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(dependencyException);

            // when
            Action generateCodeAction = () =>
                this.templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationDependencyException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateOrchestrationDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnGenerateCodeFromTemplateAndLogIt()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;
            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = inputTemplates,
                    ReplacementDictionary = inputDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            var serviceException = new Exception();

            var failedTemplateGenerationOrchestrationServiceException =
                new FailedTemplateGenerationOrchestrationServiceException(serviceException);

            var expectedTemplateGenerationOrchestrationServiceException =
                new TemplateGenerationOrchestrationServiceException(failedTemplateGenerationOrchestrationServiceException);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(serviceException);

            // when
            Action generateCodeAction = () =>
                this.templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);

            TemplateGenerationOrchestrationServiceException actualException =
                Assert.Throws<TemplateGenerationOrchestrationServiceException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationServiceException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.loggingBrokerMock.Verify(loggingBroker =>
                loggingBroker.LogInformation(It.IsAny<string>()),
                    Times.AtLeastOnce());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateGenerationOrchestrationServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
