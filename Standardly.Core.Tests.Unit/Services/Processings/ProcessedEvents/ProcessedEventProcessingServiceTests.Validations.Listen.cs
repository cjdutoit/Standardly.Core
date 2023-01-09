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
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnListenToProcessedEventIfEventHandlerIsNull()
        {
            // given
            Func<Processed, ValueTask<Processed>> processedProcessingEventHandlerMock = null;

            var nullProcessedEventProcessingHandler =
                new NullProcessedEventProcessingHandlerException();

            var expectedProcessedEventProcessingValidationException =
                new ProcessedEventProcessingValidationException(nullProcessedEventProcessingHandler);

            Action listenToProcessedEventAction = () => this.processedEventProcessingService
                .ListenToProcessedEvent(processedProcessingEventHandlerMock);

            ProcessedEventProcessingValidationException actualProcessedEventProcessingValidationException =
                Assert.Throws<ProcessedEventProcessingValidationException>(listenToProcessedEventAction);

            // when
            actualProcessedEventProcessingValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingValidationException);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.ListenToProcessedEvent(processedProcessingEventHandlerMock),
                    Times.Never);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
