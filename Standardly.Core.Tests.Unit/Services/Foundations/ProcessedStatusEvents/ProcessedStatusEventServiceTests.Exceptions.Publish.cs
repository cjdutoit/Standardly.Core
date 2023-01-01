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
        public async void ShouldThrowServiceExceptionOnPublishProcessedStatusEventIfServiceErrorOccursAsync()
        {
            // given
            ProcessedStatus randomProcessedStatus = CreateRandomProcessedStatus();
            ProcessedStatus inputProcessedStatus = randomProcessedStatus;

            var serviceException = new Exception();

            var failedProcessedStatusEventServiceException =
                new FailedProcessedStatusEventServiceException(serviceException);

            var expectedProcessedStatusEventServiceException =
                new ProcessedStatusEventServiceException(failedProcessedStatusEventServiceException);

            this.eventBrokerMock.Setup(broker =>
                broker.PublishProcessedEventAsync(inputProcessedStatus))
                    .Throws(serviceException);

            ValueTask listenToProcessedStatusEventTask = this.processedStatusEventService
                .PublishProcessedStatusAsync(inputProcessedStatus);

            ProcessedStatusEventServiceException actualProcessedStatusEventValidationException =
                await Assert.ThrowsAsync<ProcessedStatusEventServiceException>(
                    listenToProcessedStatusEventTask.AsTask);

            // when
            actualProcessedStatusEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedStatusEventServiceException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(inputProcessedStatus),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
