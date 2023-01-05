// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Standardly.Core.Models.Processings.ProcessedEvents.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnPublishIfDependencyValidationErrorOccurs(
                    Xeption dependencyValidationException)
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            var serviceException = new Exception();

            var expectedProcessedEventProcessingDependencyValidationException =
                new ProcessedEventProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.processedEventServiceMock.Setup(service =>
                service.PublishProcessedAsync(inputProcessed))
                    .Throws(dependencyValidationException);

            ValueTask publishProcessedTask = this.processedEventProcessingService
                .PublishProcessedAsync(inputProcessed);

            ProcessedEventProcessingDependencyValidationException
                actualProcessedEventProcessingDependencyValidationException =
                    await Assert.ThrowsAsync<ProcessedEventProcessingDependencyValidationException>(
                        publishProcessedTask.AsTask);

            // when
            actualProcessedEventProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingDependencyValidationException);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputProcessed),
                        Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
