// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationOnListenToProcessedEventIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            var processedEventHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            var serviceException = new Exception();

            var expectedProcessedEventProcessingDependencyValidationException =
                new ProcessedEventProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.processedEventServiceMock.Setup(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object))
                    .Throws(dependencyValidationException);

            Action listenToProcessedEventAction = () => this.processedEventProcessingService
                .ListenToProcessedEvent(processedEventHandlerMock.Object);

            ProcessedEventProcessingDependencyValidationException
                actualProcessedEventProcessingDependencyValidationException =
                    Assert.Throws<ProcessedEventProcessingDependencyValidationException>(listenToProcessedEventAction);

            // when
            actualProcessedEventProcessingDependencyValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingDependencyValidationException);

            // then
            this.processedEventServiceMock.Verify(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyOnListenToProcessedEventIfDependencyErrorOccurs(
           Xeption dependencyException)
        {
            // given
            var processedEventHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            var serviceException = new Exception();

            var expectedProcessedEventProcessingDependencyException =
                new ProcessedEventProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.processedEventServiceMock.Setup(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object))
                    .Throws(dependencyException);

            // when
            Action listenToProcessedEventAction = () => this.processedEventProcessingService
                .ListenToProcessedEvent(processedEventHandlerMock.Object);

            ProcessedEventProcessingDependencyException
                actualProcessedEventProcessingDependencyException =
                    Assert.Throws<ProcessedEventProcessingDependencyException>(listenToProcessedEventAction);

            // then
            actualProcessedEventProcessingDependencyException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingDependencyException);

            this.processedEventServiceMock.Verify(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnListenToProcessedEventIfServiceErrorOccurs()
        {
            // given
            var processedEventHandlerMock =
                new Mock<Func<Processed, ValueTask<Processed>>>();

            var serviceException = new Exception();

            var failedProcessedEventProcessingServiceException =
                new FailedProcessedEventProcessingServiceException(serviceException);

            var expectedProcessedEventProcessingServiceException =
                new ProcessedEventProcessingServiceException(failedProcessedEventProcessingServiceException);

            this.processedEventServiceMock.Setup(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object))
                    .Throws(serviceException);

            // when
            Action listenToProcessedEventAction = () => this.processedEventProcessingService
                .ListenToProcessedEvent(processedEventHandlerMock.Object);

            ProcessedEventProcessingServiceException actualProcessedEventProcessingServiceException =
                Assert.Throws<ProcessedEventProcessingServiceException>(listenToProcessedEventAction);

            // then
            actualProcessedEventProcessingServiceException.Should()
                .BeEquivalentTo(expectedProcessedEventProcessingServiceException);

            this.processedEventServiceMock.Verify(service =>
                service.ListenToProcessedEvent(processedEventHandlerMock.Object),
                    Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
