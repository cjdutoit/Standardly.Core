// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Moq;
using Standardly.Core.Brokers.Events;
using Standardly.Core.Models.Events.ProcessedStatuses;
using Standardly.Core.Services.Foundations.ProcessedStatusEvents;
using Tynamix.ObjectFiller;

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

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static ProcessedStatus CreateRandomProcessedStatus(DateTimeOffset? dateTimeOffset = null) =>
            CreateProcessedStatusFiller(dateTimeOffset ?? GetRandomDateTimeOffset()).Create();

        private static Filler<ProcessedStatus> CreateProcessedStatusFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<ProcessedStatus>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(borough => borough.Message).Use(GetRandomString())
                .OnProperty(borough => borough.Status).Use(GetRandomString())
                .OnProperty(borough => borough.ProcessedItems).Use(GetRandomNumber())
                .OnProperty(borough => borough.TotalItems).Use(GetRandomNumber());

            return filler;
        }
    }
}
