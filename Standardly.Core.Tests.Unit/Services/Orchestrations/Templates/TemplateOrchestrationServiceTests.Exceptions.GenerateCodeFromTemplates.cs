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
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
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

            var expectedDependencyValidationException =
                new TemplateOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(dependencyValidationException);

            // when
            Action generateCodeAction = () =>
                this.templateOrchestrationService.GenerateCode(inputTemplates, inputDictionary);

            TemplateOrchestrationDependencyValidationException actualException =
                Assert.Throws<TemplateOrchestrationDependencyValidationException>(
                    generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), inputDictionary),
                    Times.Once);

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

            var expectedTemplateOrchestrationDependencyException =
                new TemplateGenerationOrchestrationDependencyException(dependencyException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(dependencyException);

            // when
            Action generateCodeAction = () =>
                this.templateOrchestrationService.GenerateCode(inputTemplates, inputDictionary);

            TemplateGenerationOrchestrationDependencyException actualException =
                Assert.Throws<TemplateGenerationOrchestrationDependencyException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationDependencyException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), inputDictionary),
                    Times.Once);

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
            var serviceException = new Exception();

            var failedTemplateOrchestrationServiceException =
                new FailedTemplateGenerationOrchestrationServiceException(serviceException);

            var expectedTemplateOrchestrationServiceException =
                new TemplateOrchestrationServiceException(failedTemplateOrchestrationServiceException);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplate(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .Throws(serviceException);

            // when
            Action generateCodeAction = () =>
                this.templateOrchestrationService.GenerateCode(inputTemplates, inputDictionary);

            TemplateOrchestrationServiceException actualException =
                Assert.Throws<TemplateOrchestrationServiceException>(generateCodeAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationServiceException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplate(It.IsAny<Template>(), inputDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
