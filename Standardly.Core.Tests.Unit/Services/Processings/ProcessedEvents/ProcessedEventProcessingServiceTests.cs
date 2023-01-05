// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Moq;
using Standardly.Core.Models.Foundations.ProcessedEvents;
using Standardly.Core.Models.Foundations.ProcessedEvents.Exceptions;
using Standardly.Core.Services.Foundations.ProcessedEvents;
using Standardly.Core.Services.Processings.ProcessedEvents;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventValidationException(innerException),
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Processed CreateRandomProcessed(DateTimeOffset? dateTimeOffset = null) =>
            CreateProcessedFiller(dateTimeOffset ?? GetRandomDateTimeOffset()).Create();

        private static Filler<Processed> CreateProcessedFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<Processed>();

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
