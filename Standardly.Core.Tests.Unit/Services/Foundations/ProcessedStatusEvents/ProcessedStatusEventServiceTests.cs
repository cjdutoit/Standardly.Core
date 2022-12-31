// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Brokers.Events;
using Standardly.Core.Services.Foundations.ProcessedStatusEvents;

namespace Standardly.Core.Tests.Unit.Services.Foundations.ProcessedStatusEvents
{
    public partial class ProcessedStatusEventServiceTests
    {
        private readonly Mock<IEventBroker> eventBrokerMock;
        private readonly IProcessedStatusEventService processedStatusEventService;

        public ProcessedStatusEventServiceTests()
        {
            this.eventBrokerMock = new Mock<IEventBroker>();

            this.processedStatusEventService = new ProcessedStatusEventService(
                eventBroker: this.eventBrokerMock.Object);
        }
    }
}
