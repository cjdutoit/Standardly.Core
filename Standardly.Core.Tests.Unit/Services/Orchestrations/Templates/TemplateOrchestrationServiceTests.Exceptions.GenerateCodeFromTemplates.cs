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
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnGenerateCodeFromTemplateAndLogItAsync(
            Exception dependencyValidationException)
        {
            // given
            int randomNumber = 1; //GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;

            var expectedDependencyValidationException =
                new TemplateOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplateAsync(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask generateCodeTask =
                this.templateOrchestrationService.GenerateCodeAsync(inputTemplates, inputDictionary);

            TemplateOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateOrchestrationDependencyValidationException>(
                    generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplateAsync(It.IsAny<Template>(), inputDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TemplateOrchestrationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGenerateCodeFromTemplateAndLogItAsync(
            Exception dependencyException)
        {
            // given
            int randomNumber = 1; //GetRandomNumber();
            List<Template> randomTemplates = GetRandomTemplateList(randomNumber, true);
            List<Template> inputTemplates = randomTemplates;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputDictionary = randomReplacementDictionary;

            var expectedTemplateOrchestrationDependencyException =
                new TemplateOrchestrationDependencyException(dependencyException.InnerException as Xeption);

            this.templateProcessingServiceMock.Setup(service =>
                service.TransformTemplateAsync(
                    It.IsAny<Template>(),
                    inputDictionary))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask generateCodeTask =
                this.templateOrchestrationService.GenerateCodeAsync(inputTemplates, inputDictionary);

            TemplateOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TemplateOrchestrationDependencyException>(generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationDependencyException);

            this.templateProcessingServiceMock.Verify(broker =>
                broker.TransformTemplateAsync(It.IsAny<Template>(), inputDictionary),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
