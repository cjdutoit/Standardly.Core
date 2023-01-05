// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
