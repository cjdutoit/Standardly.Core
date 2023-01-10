// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnPublishIfProcessedIsNullAsync()
        {
            // given
            Processed nullProcessed = null;

            var nullProcessedEventException = new NullProcessedEventException();

            var expectedProcessedEventValidationException =
                new ProcessedEventValidationException(nullProcessedEventException);

            // when
            ValueTask publishProcessedTask =
                this.processedEventService.PublishProcessedAsync(nullProcessed);

            ProcessedEventValidationException actualProcessedEventValidationException =
                await Assert.ThrowsAsync<ProcessedEventValidationException>(publishProcessedTask.AsTask);

            // then
            actualProcessedEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventValidationException);

            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(nullProcessed),
                    Times.Never);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
