// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Fact]
        public async Task ShouldPublishProcessedAsync()
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            // when
            await this.processedEventProcessingService
                .PublishProcessedAsync(inputProcessed);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputProcessed),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
