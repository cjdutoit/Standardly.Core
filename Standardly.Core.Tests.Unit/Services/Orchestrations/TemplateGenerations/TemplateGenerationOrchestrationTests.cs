// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Processings.ProcessedEvents;
using Standardly.Core.Services.Processings.Templates;

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
    }
}
