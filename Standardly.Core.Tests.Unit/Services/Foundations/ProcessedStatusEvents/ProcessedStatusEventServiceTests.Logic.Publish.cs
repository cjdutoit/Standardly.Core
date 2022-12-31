// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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

            // when
            await this.processedStatusEventService
                .PublishProcessedStatusAsync(inputProcessedStatus);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(inputProcessedStatus),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
