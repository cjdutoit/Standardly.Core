// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(ProcessedEventsDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnPublishIfDependencyValidationErrorOccursAsync(
                     Xeption dependencyValidationException)
        {
            // given
            TemplateGenerationInfo randomTemplateGenerationInfo = CreateRandomTemplateGenerationInfo();
            TemplateGenerationInfo inputTemplateGenerationInfo = randomTemplateGenerationInfo;

            var serviceException = new Exception();

            var expectedProcessedEventProcessingDependencyValidationException =
                new ProcessedEventProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.processedEventProcessingServiceMock.Setup(service =>
                service.PublishProcessedAsync(inputTemplateGenerationInfo.Processed))
                    .Throws(dependencyValidationException);

            // when
            ValueTask publishProcessedTask = this.templateGenerationOrchestrationService
                .PublishProcessedAsync(inputTemplateGenerationInfo);

            ProcessedEventProcessingDependencyValidationException
                actualProcessedEventProcessingDependencyValidationException =
                    await Assert.ThrowsAsync<ProcessedEventProcessingDependencyValidationException>(
                        publishProcessedTask.AsTask);

            // then
            actualProcessedEventProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingDependencyValidationException);

            this.processedEventProcessingServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputTemplateGenerationInfo.Processed),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
