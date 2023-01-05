// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Services.Foundations.ProcessedEvents;
using Standardly.Core.Services.Processings.ProcessedEvents;

namespace Standardly.Core.Tests.Unit.Services.Processings.ProcessedEvents
{
    public partial class ProcessedEventProcessingServiceTests
    {
        private readonly Mock<IProcessedEventService> processedEventServiceMock;
        private readonly IProcessedEventProcessingService processedEventProcessingService;

        public ProcessedEventProcessingServiceTests()
        {
            this.processedEventServiceMock = new Mock<IProcessedEventService>();

            this.processedEventProcessingService = new ProcessedEventProcessingService(
                processedEventService: this.processedEventServiceMock.Object);
        }
    }
}
