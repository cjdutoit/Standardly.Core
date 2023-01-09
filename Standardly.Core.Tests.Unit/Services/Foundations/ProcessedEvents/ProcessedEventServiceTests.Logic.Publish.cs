// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedEvents
{
    public partial class ProcessedEventServiceTests
    {
        [Fact]
        public async Task ShouldPublishProcessedAsync()
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            // when
            await this.processedEventService
                .PublishProcessedAsync(inputProcessed);

            // then
            this.eventBrokerMock.Verify(broker =>
                broker.PublishProcessedEventAsync(inputProcessed),
                    Times.Once);

            this.eventBrokerMock.VerifyNoOtherCalls();
        }
    }
}
