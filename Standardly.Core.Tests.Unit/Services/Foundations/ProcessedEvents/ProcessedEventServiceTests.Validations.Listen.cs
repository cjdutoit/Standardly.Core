// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnListenToProcessedEventIfEventHandlerIsNull()
        {
            // given
            Func<Processed, ValueTask<Processed>> processedEventHandlerMock = null;

            var nullProcessedEventHandler =
                new NullProcessedEventHandler();

            var expectedProcessedEventValidationException =
                new ProcessedEventValidationException(nullProcessedEventHandler);

            Action listenToProcessedEventAction = () => this.processedEventService
                .ListenToProcessedEvent(processedEventHandlerMock);

            ProcessedEventValidationException actualProcessedEventValidationException =
                Assert.Throws<ProcessedEventValidationException>(listenToProcessedEventAction);

            // when
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventValidationException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(
                    processedEventHandlerMock), Times.Never);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
