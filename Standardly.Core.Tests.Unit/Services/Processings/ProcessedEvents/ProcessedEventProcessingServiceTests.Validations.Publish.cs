// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Processings.Executions.Exceptions;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnPublishIfProcessedIsNullAsync()
        {
            // given
            Processed nullProcessed = null;

            var nullProcessedEventProcessingException = new NullProcessedEventProcessingException();

            var expectedProcessedEventProcessingValidationException =
                new ProcessedEventProcessingValidationException(nullProcessedEventProcessingException);

            // when
            ValueTask publishProcessedTask =
                this.processedEventProcessingService.PublishProcessedAsync(nullProcessed);

            ProcessedEventProcessingValidationException actualProcessedEventProcessingValidationException =
                await Assert.ThrowsAsync<ProcessedEventProcessingValidationException>(publishProcessedTask.AsTask);

            // then
            actualProcessedEventProcessingValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingValidationException);

            this.processedEventServiceMock.Verify(service =>
                service.PublishProcessedAsync(nullProcessed),
                    Times.Never);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
