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
        public async void ShouldThrowServiceExceptionOnListenToProcessedEventIfServiceErrorOccurs()
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

            Action listenToProcessedEventAction = () => this.processedEventService
                .ListenToProcessedEvent(processedEventHandlerMock.Object);

            ProcessedEventServiceException actualProcessedEventValidationException =
                Assert.Throws<ProcessedEventServiceException>(listenToProcessedEventAction);

            // when
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventServiceException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(processedEventHandlerMock.Object),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
