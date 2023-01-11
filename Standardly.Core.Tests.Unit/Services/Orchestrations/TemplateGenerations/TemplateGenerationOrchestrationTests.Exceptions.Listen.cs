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
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(ProcessedEventsDependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationOnListenToProcessedEventIfDependencyValidationErrorOccurs(
            Xeption dependencyValidationException)
        {
            // given
            var processedEventOrchestrationHandlerMock =
                new Mock<Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>>>();

            var serviceException = new Exception();

            var expectedProcessedEventOrchestrationDependencyValidationException =
                new ProcessedEventOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.processedEventProcessingServiceMock.Setup(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()))
                    .Throws(dependencyValidationException);

            Action listenToProcessedEventAction = () => this.templateGenerationOrchestrationService
                .ListenToProcessedEvent(processedEventOrchestrationHandlerMock.Object);

            ProcessedEventOrchestrationDependencyValidationException
                actualProcessedEventOrchestrationDependencyValidationException =
                    Assert.Throws<ProcessedEventOrchestrationDependencyValidationException>(listenToProcessedEventAction);

            // when
            actualProcessedEventOrchestrationDependencyValidationException.Should()
                .BeEquivalentTo(expectedProcessedEventOrchestrationDependencyValidationException);

            // then
            this.processedEventProcessingServiceMock.Verify(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ProcessedEventsDependencyExceptions))]
        public void ShouldThrowDependencyOnListenToProcessedEventIfDependencyErrorOccurs(
            Xeption dependencyException)
        {
            // given
            var processedEventOrchestrationHandlerMock =
                new Mock<Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>>>();

            var serviceException = new Exception();

            var expectedProcessedEventOrchestrationDependencyException =
                new ProcessedEventOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.processedEventProcessingServiceMock.Setup(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()))
                    .Throws(dependencyException);

            Action listenToProcessedEventAction = () => this.templateGenerationOrchestrationService
                .ListenToProcessedEvent(processedEventOrchestrationHandlerMock.Object);

            ProcessedEventOrchestrationDependencyException
                actualProcessedEventOrchestrationDependencyException =
                    Assert.Throws<ProcessedEventOrchestrationDependencyException>(listenToProcessedEventAction);

            // when
            actualProcessedEventOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedProcessedEventOrchestrationDependencyException);

            // then
            this.processedEventProcessingServiceMock.Verify(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnListenToProcessedEventIfServiceErrorOccurs()
        {
            var processedEventOrchestrationHandlerMock =
                new Mock<Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>>>();

            var serviceException = new Exception();

            var failedProcessedEventOrchestrationServiceException =
                new FailedProcessedEventOrchestrationServiceException(serviceException);

            var expectedProcessedEventOrchestrationServiceException =
                new ProcessedEventOrchestrationServiceException(failedProcessedEventOrchestrationServiceException);

            this.processedEventProcessingServiceMock.Setup(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()))
                    .Throws(serviceException);

            Action listenToProcessedEventAction = () => this.templateGenerationOrchestrationService
                .ListenToProcessedEvent(processedEventOrchestrationHandlerMock.Object);

            ProcessedEventOrchestrationServiceException
                actualProcessedEventOrchestrationDependencyException =
                    Assert.Throws<ProcessedEventOrchestrationServiceException>(listenToProcessedEventAction);

            // when
            actualProcessedEventOrchestrationDependencyException.Should()
                .BeEquivalentTo(expectedProcessedEventOrchestrationServiceException);

            // then
            this.processedEventProcessingServiceMock.Verify(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
