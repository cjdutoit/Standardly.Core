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
        public async Task ShouldThrowDependencyValidationOnListenToProcessedEventIfDependencyValidationErrorOccurs(
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
                service.ListenToProcessedEvent(
                    processedEventHandlerMock.Object),
                        Times.Once);

            this.processedEventServiceMock.VerifyNoOtherCalls();
        }
    }
}
