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
        }
    }
}
