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
        public async void ShouldThrowValidationExceptionOnListenToProcessedStatusEventIfEventHandlerIsNull()
        {
            // given
            Func<ProcessedStatus, ValueTask<ProcessedStatus>> processedStatusEventHandlerMock = null;

            var nullProcessedStatusEventHandler =
                new NullProcessedStatusEventHandler();

            var expectedProcessedStatusEventValidationException =
                new ProcessedStatusEventValidationException(nullProcessedStatusEventHandler);

            Action listenToProcessedStatusEventAction = () => this.processedStatusEventService
                .ListenToProcessedStatusEvent(processedStatusEventHandlerMock);

            ProcessedStatusEventValidationException actualProcessedStatusEventValidationException =
                Assert.Throws<ProcessedStatusEventValidationException>(listenToProcessedStatusEventAction);

            // when
            actualProcessedStatusEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedStatusEventValidationException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.ListenToProcessedEvent(
                    processedStatusEventHandlerMock), Times.Never);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
