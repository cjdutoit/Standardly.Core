// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Events.ProcessedStatuses;
using Standardly.Core.Models.Events.ProcessedStatuses.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventServiceTests
    {
        [Fact]
        public async void ShouldThrowServiceExceptionOnListenToProcessedStatusEventIfServiceErrorOccurs()
        {
            // given
            var processedStatusEventHandlerMock =
                new Mock<Func<ProcessedStatus, ValueTask<ProcessedStatus>>>();

            var serviceException = new Exception();

            var failedProcessedStatusEventServiceException =
                new FailedProcessedStatusEventServiceException(serviceException);

            var expectedProcessedStatusEventServiceException =
                new ProcessedStatusEventServiceException(failedProcessedStatusEventServiceException);

            this.eventBrokerMock.Setup(broker =>
                broker.ListenToProcessedEvent(processedStatusEventHandlerMock.Object))
                    .Throws(serviceException);

            Action listenToProcessedStatusEventAction = () => this.processedStatusEventService
                .ListenToProcessedStatusEvent(processedStatusEventHandlerMock.Object);

            ProcessedStatusEventServiceException actualProcessedStatusEventValidationException =
                Assert.Throws<ProcessedStatusEventServiceException>(listenToProcessedStatusEventAction);

            // when
            actualProcessedStatusEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedStatusEventServiceException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(
                    processedStatusEventHandlerMock.Object),
                        Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
