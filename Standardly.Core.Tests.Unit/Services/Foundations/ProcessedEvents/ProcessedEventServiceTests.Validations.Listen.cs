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
using Standardly.Core.Models.Services.Foundations.ProcessedEvents.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnListenToProcessedEventIfEventHandlerIsNull()
        {
            // given
            Func<Processed, ValueTask<Processed>> processedEventHandlerMock = null;

            var nullProcessedEventHandler =
                new NullProcessedEventHandlerException();

            var expectedProcessedEventValidationException =
                new ProcessedEventValidationException(nullProcessedEventHandler);

            // when
            Action listenToProcessedEventAction = () => this.processedEventService
                .ListenToProcessedEvent(processedEventHandlerMock);

            ProcessedEventValidationException actualProcessedEventValidationException =
                Assert.Throws<ProcessedEventValidationException>(listenToProcessedEventAction);

            // then
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventValidationException);

            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(processedEventHandlerMock),
                    Times.Never);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
