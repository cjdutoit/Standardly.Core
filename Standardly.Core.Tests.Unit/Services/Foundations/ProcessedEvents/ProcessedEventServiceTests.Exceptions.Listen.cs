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
        public void ShouldThrowServiceExceptionOnListenToProcessedEventIfServiceErrorOccurs()
        {
            // given
            var processedEventHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            var serviceException = new Exception();

            var failedProcessedServiceException =
                new FailedProcessedEventServiceException(serviceException);

            var expectedProcessedEventServiceException =
                new ProcessedEventServiceException(failedProcessedServiceException);

            this.eventBrokerMock.Setup(broker =>
                broker.ListenToProcessedEvent(processedEventHandlerMock.Object))
                    .Throws(serviceException);

            // when
            Action listenToProcessedEventAction = () => this.processedEventService
                .ListenToProcessedEvent(processedEventHandlerMock.Object);

            ProcessedEventServiceException actualProcessedEventValidationException =
                Assert.Throws<ProcessedEventServiceException>(listenToProcessedEventAction);

            // then
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventServiceException);

            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(processedEventHandlerMock.Object),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
