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
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(InvalidCharacters))]
        public async Task ShouldThrowValidationExceptionIfArgumentsIsInvalidAndLogItAsync(char invalidInput)
        {
            // given
            char invalidTagCharacter = invalidInput;
            Template nullTemplate = null;
            Dictionary<string, string> nullDictionary = null;

            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            invalidArgumentTemplateProcessingException.AddData(
                key: "template",
                values: "Template is required");

            invalidArgumentTemplateProcessingException.AddData(
                key: "replacementDictionary",
                values: "Dictionary values is required");

            invalidArgumentTemplateProcessingException.AddData(
                key: "tagCharacter",
                values: "Character is required");

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(nullTemplate, nullDictionary, invalidTagCharacter);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingValidationException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(It.IsAny<string>(), nullDictionary),
                    Times.Never);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>(), invalidTagCharacter),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
