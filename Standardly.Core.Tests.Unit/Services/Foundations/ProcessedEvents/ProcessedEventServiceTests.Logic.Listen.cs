// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventServiceTests
    {
        [Fact]
        public void ShouldListenToProcessedEvents()
        {
            // given
            var processedEventHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            // when
            this.processedEventService.ListenToProcessedEvent(
                processedEventHandlerMock.Object);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(
                    processedEventHandlerMock.Object), Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
