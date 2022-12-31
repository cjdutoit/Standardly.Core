// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Standardly.Core.Models.Events.ProcessedStatuses;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventServiceTests
    {
        [Fact]
        public async Task ShouldPublishProcessedStatusAsync()
        {
            // given
            ProcessedStatus randomProcessedStatus = CreateRandomProcessedStatus();
            ProcessedStatus inputProcessedStatus = randomProcessedStatus;
            ProcessedStatus expectedProcessedStatus = inputProcessedStatus.DeepClone();

            // when
            ProcessedStatus actualProcessedStatus =
                await this.processedStatusEventService
                    .PublishProcessedStatusAsync(inputProcessedStatus);

            // then
            actualProcessedStatus.Should().BeEquivalentTo(expectedProcessedStatus);

            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(inputProcessedStatus),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
