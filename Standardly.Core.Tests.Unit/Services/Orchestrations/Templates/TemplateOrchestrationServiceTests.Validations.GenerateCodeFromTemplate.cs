// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfArgumentsIsNull()
        {
            // given
            List<Template> nullTemplateList = null;
            Dictionary<string, string> randomReplacementDictionary = null;

            var invalidArgumentTemplateOrchestrationException =
                new InvalidArgumentTemplateOrchestrationException();

            invalidArgumentTemplateOrchestrationException.AddData(
                key: "templates",
                values: "Templates is required");

            invalidArgumentTemplateOrchestrationException.AddData(
                key: "replacementDictionary",
                values: "Dictionary values is required");

            this.templateProcessingServiceMock.Setup(templateProcessingService =>
                templateProcessingService
                    .TransformTemplateAsync(It.IsAny<Template>(), It.IsAny<Dictionary<string, string>>()))
                        .Throws(invalidArgumentTemplateOrchestrationException);

            var expectedTemplateOrchestrationValidationException =
                new TemplateOrchestrationValidationException(invalidArgumentTemplateOrchestrationException);

            // when
            ValueTask generateCodeTask =
               templateOrchestrationService.GenerateCodeAsync(nullTemplateList, randomReplacementDictionary);

            TemplateOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<TemplateOrchestrationValidationException>(generateCodeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateOrchestrationValidationException);

            this.templateProcessingServiceMock.VerifyNoOtherCalls();
            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
