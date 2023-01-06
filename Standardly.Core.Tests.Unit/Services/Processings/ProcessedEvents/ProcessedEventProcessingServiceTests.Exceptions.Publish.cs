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
        public async Task ShouldThrowDependencyValidationOnPublishIfDependencyValidationErrorOccursAsync(
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

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnPublishIfDependencyErrorOccursAsync(
           Xeption dependencyException)
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            var serviceException = new Exception();

            var expectedProcessedEventProcessingDependencyException =
                new ProcessedEventProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.processedEventServiceMock.Setup(service =>
                service.PublishProcessedAsync(inputProcessed))
                    .Throws(dependencyException);

            ValueTask publishProcessedTask = this.processedEventProcessingService
                .PublishProcessedAsync(inputProcessed);

            ProcessedEventProcessingDependencyException
                actualProcessedEventProcessingDependencyException =
                    await Assert.ThrowsAsync<ProcessedEventProcessingDependencyException>(
                        publishProcessedTask.AsTask);

            // when
            actualProcessedEventProcessingDependencyException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingDependencyException);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputProcessed),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnPublishIfServiceErrorOccursAsync()
        {
            // given
            Processed randomProcessed = CreateRandomProcessed();
            Processed inputProcessed = randomProcessed;

            var serviceException = new Exception();

            var failedProcessedEventProcessingServiceException =
                new FailedProcessedEventProcessingServiceException(serviceException);

            var expectedProcessedEventProcessingServiceException =
                new ProcessedEventProcessingServiceException(failedProcessedEventProcessingServiceException);

            this.processedEventServiceMock.Setup(service =>
                service.PublishProcessedAsync(inputProcessed))
                    .Throws(serviceException);

            ValueTask publishProcessedTask = this.processedEventProcessingService
                .PublishProcessedAsync(inputProcessed);

            ProcessedEventProcessingServiceException actualProcessedEventProcessingServiceException =
                await Assert.ThrowsAsync<ProcessedEventProcessingServiceException>(publishProcessedTask.AsTask);

            // when
            actualProcessedEventProcessingServiceException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingServiceException);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputProcessed),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
