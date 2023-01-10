// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowValidationExceptionOnListenToProcessedEventIfEventHandlerIsNull()
        {
            // given
            Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>> processedOrchestrationEventHandler = null;

            var nullProcessedEventOrchestrationHandler =
                new NullProcessedEventOrchestrationHandlerException();

            var expectedProcessedEventOrchestrationValidationException =
                new ProcessedEventOrchestrationValidationException(nullProcessedEventOrchestrationHandler);

            // when
            Action listenToProcessedEventAction = () => this.templateGenerationOrchestrationService
                .ListenToProcessedEvent(processedOrchestrationEventHandler);

            ProcessedEventOrchestrationValidationException actualProcessedEventOrchestrationValidationException =
                Assert.Throws<ProcessedEventOrchestrationValidationException>(listenToProcessedEventAction);

            // then
            actualProcessedEventOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventOrchestrationValidationException);

            this.processedEventProcessingServiceMock.Verify(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
                    Times.Never);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
