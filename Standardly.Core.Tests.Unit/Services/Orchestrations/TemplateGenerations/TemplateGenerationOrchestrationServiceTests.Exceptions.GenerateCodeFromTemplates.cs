// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnGenerateCodeFromTemplateAsync(
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
                service.TransformTemplateAsync(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask generateCodeTask =
                this.templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationDependencyValidationException>(
                    generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplateAsync(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGenerateCodeFromTemplateAsync(
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
                service.TransformTemplateAsync(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask generateCodeTask =
                this.templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);

            TemplateGenerationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationDependencyException>(generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationDependencyException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplateAsync(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnGenerateCodeFromTemplateAsync()
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
                service.TransformTemplateAsync(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask generateCodeTask =
                this.templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);

            TemplateGenerationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TemplateGenerationOrchestrationServiceException>(generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateGenerationOrchestrationServiceException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplateAsync(It.IsAny<Template>(), templateGenerationInfo.ReplacementDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
