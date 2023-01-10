// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        //[Fact]
        //public void ShouldThrowValidationExceptionOnListenToProcessedEventIfEventHandlerIsNull()
        //{
        //    // given
        //    var processedOrchestrationEventHandlerMock =
        //        new Mock<Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>>>();

        //    var nullProcessedEventOrchestrationHandler =
        //        new NullProcessedEventOrchestrationHandlerException();

        //    var expectedProcessedEventOrchestrationValidationException =
        //        new ProcessedEventOrchestrationValidationException(nullProcessedEventOrchestrationHandler);

        //    Action listenToProcessedEventAction = () => this.templateGenerationOrchestrationService
        //        .ListenToProcessedEvent(processedOrchestrationEventHandlerMock.Object);

        //    ProcessedEventOrchestrationValidationException actualProcessedEventOrchestrationValidationException =
        //        Assert.Throws<ProcessedEventOrchestrationValidationException>(listenToProcessedEventAction);

        //    // when
        //    actualProcessedEventOrchestrationValidationException.Should()
        //        .BeEquivalentTo(expectedProcessedEventOrchestrationValidationException);

        //    // then
        //    this.processedEventProcessingServiceMock.Verify(service =>
        //        service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
        //            Times.Never);

        //    this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
        //    this.templateProcessingServiceMock.VerifyNoOtherCalls();
        //}
    }
}
