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

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Fact]
        public void ShouldListenToProcessedEvents()
        {
            // given
            var processedEventProcessingHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            // when
            this.processedEventProcessingService.ListenToProcessedEvent(
                processedEventProcessingHandlerMock.Object);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.ListenToProcessedEvent(
                    processedEventProcessingHandlerMock.Object), Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
