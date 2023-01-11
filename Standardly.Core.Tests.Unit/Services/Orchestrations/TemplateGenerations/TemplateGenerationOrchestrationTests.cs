// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Standardly.Core.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Processings.ProcessedEvents;
using Standardly.Core.Services.Processings.Templates;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        private readonly Mock<IProcessedEventProcessingService> processedEventProcessingServiceMock;
        private readonly Mock<ITemplateProcessingService> templateProcessingServiceMock;
        private readonly ITemplateGenerationOrchestrationService templateGenerationOrchestrationService;

        public TemplateGenerationOrchestrationTests()
        {
            this.processedEventProcessingServiceMock = new Mock<IProcessedEventProcessingService>();
            this.templateProcessingServiceMock = new Mock<ITemplateProcessingService>();

            this.templateGenerationOrchestrationService = new TemplateGenerationOrchestrationService(
                processedEventProcessingService: this.processedEventProcessingServiceMock.Object,
                templateProcessingService: this.templateProcessingServiceMock.Object);
        }

        public static TheoryData ProcessedEventsDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventProcessingValidationException(innerException),
                new ProcessedEventProcessingDependencyValidationException(innerException),
            };
        }

        public static TheoryData ProcessedEventsDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventProcessingDependencyException(innerException),
                new ProcessedEventProcessingServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
