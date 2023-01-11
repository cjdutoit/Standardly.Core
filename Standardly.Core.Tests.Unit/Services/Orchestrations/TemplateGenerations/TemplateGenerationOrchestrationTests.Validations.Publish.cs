// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnPublishIfProcessedIsNullAsync()
        {
            // given
            TemplateGenerationInfo nullTemplateGenerationInfo = null;

            var nullTemplateGenerationInfoOrchestrationException =
                new NullTemplateGenerationInfoOrchestrationException();

            var expectedProcessedEventOrchestrationValidationException =
                new ProcessedEventOrchestrationValidationException(nullTemplateGenerationInfoOrchestrationException);

            // when
            ValueTask publishProcessedTask =
                this.templateGenerationOrchestrationService.PublishProcessedAsync(nullTemplateGenerationInfo);

            ProcessedEventOrchestrationValidationException actualProcessedEventOrchestrationValidationException =
                await Assert.ThrowsAsync<ProcessedEventOrchestrationValidationException>(publishProcessedTask.AsTask);

            // then
            actualProcessedEventOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventOrchestrationValidationException);

            this.processedEventProcessingServiceMock.Verify(service =>
                service.PublishProcessedAsync(It.IsAny<Processed>()),
                    Times.Never);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
