// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        [Fact]
        public void ShouldListenToProcessedEvents()
        {
            // given
            var processedEventOrchestrationHandlerMock =
                new Mock<Func<TemplateGenerationInfo, ValueTask<TemplateGenerationInfo>>>();


            Processed processed = CreateRandomProcessed();

            TemplateGenerationInfo randomTemplateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Processed = processed
                };

            TemplateGenerationInfo inputTemplateGenerationInfo = randomTemplateGenerationInfo;

            this.processedEventProcessingServiceMock.Setup(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()))
                    .Callback<Func<Processed, ValueTask<Processed>>>(processedFunction =>
                        processedFunction.Invoke(processed));

            // when
            this.templateGenerationOrchestrationService.ListenToProcessedEvent(
                processedEventOrchestrationHandlerMock.Object);

            // then
            processedEventOrchestrationHandlerMock.Verify(handler =>
                handler.Invoke(It.Is(SameTemplateGenerationInfoAs(inputTemplateGenerationInfo))),
                    Times.Once);

            this.processedEventProcessingServiceMock.Verify(service =>
                service.ListenToProcessedEvent(It.IsAny<Func<Processed, ValueTask<Processed>>>()),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
