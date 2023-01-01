// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnPublishIfProcessedStatusIsNull()
        {
            // given
            ProcessedStatus nullProcessedStatus = null;

            var nullProcessedStatusException = new NullProcessedStatusException();

            var expectedProcessedStatusEventValidationException =
                new ProcessedStatusEventValidationException(nullProcessedStatusException);

            ValueTask publishProcessedStatusTask =
                this.processedStatusEventService.PublishProcessedStatusAsync(nullProcessedStatus);

            ProcessedStatusEventValidationException actualProcessedStatusEventValidationException =
                await Assert.ThrowsAsync<ProcessedStatusEventValidationException>(publishProcessedStatusTask.AsTask);

            // when
            actualProcessedStatusEventValidationException.Should()
                .BeEquivalentTo(expectedProcessedStatusEventValidationException);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(nullProcessedStatus),
                    Times.Never);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
