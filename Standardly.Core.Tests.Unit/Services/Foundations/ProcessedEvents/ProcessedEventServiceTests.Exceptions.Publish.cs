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
        public async Task ShouldThrowServiceExceptionOnPublishProcessedEventIfServiceErrorOccursAsync()
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            var serviceException = new Exception();

            var failedProcessedEventServiceException =
                new FailedProcessedEventServiceException(serviceException);

            var expectedProcessedEventServiceException =
                new ProcessedEventServiceException(failedProcessedEventServiceException);

            this.eventBrokerMock.Setup(broker =>
                broker.PublishProcessedEventAsync(inputProcessed))
                    .Throws(serviceException);

            // when
            ValueTask listenToProcessedEventTask = this.processedEventService
                .PublishProcessedAsync(inputProcessed);

            ProcessedEventServiceException actualProcessedEventValidationException =
                await Assert.ThrowsAsync<ProcessedEventServiceException>(
                    listenToProcessedEventTask.AsTask);

            // then
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventServiceException);

            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(inputProcessed),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
