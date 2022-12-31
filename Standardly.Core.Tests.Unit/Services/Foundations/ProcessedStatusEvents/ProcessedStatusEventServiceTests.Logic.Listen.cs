// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Events;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventServiceTests
    {
        [Fact]
        public void ShouldListenToProcessedStatusEvents()
        {
            // given
            var processedStatusEventHandlerMock =
                new Mock<Func<ProcessedStatus, ValueTask<ProcessedStatus>>>();

            // when
            this.processedStatusEventService.ListenToProcessedStatusEvent(
                processedStatusEventHandlerMock.Object);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(
                    processedStatusEventHandlerMock.Object), Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
