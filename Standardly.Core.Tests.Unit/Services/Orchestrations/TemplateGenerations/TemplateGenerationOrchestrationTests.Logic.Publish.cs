// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        [Fact]
        public async Task ShouldPublishProcessedAsync()
        {
            // given
            TemplateGenerationInfo randomTemplateGenerationInfo = CreateRandomTemplateGenerationInfo();
            TemplateGenerationInfo inputTemplateGenerationInfo = randomTemplateGenerationInfo;

            // when
            await this.templateGenerationOrchestrationService
                .PublishProcessedAsync(inputTemplateGenerationInfo);

            // then
            this.processedEventProcessingServiceMock.Verify(service =>
                service.PublishProcessedAsync(inputTemplateGenerationInfo.Processed),
                    Times.Once);

            this.processedEventProcessingServiceMock.VerifyNoOtherCalls();
            this.templateProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
