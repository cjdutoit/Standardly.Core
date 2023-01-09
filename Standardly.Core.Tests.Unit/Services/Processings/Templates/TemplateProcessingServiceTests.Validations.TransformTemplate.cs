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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnTransformTemplateIfTemplateIsNullAsync()
        {
            // given
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

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(nullTemplate);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()),
                    Times.Never);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnTransformTemplateIfDictionaryIsNullAsync()
        {
            // given
            Template randomTemplate = CreateRandomTemplate();
            Dictionary<string, string> nullDictionary = null;
            Template inputTemplate = randomTemplate;
            inputTemplate.ReplacementDictionary = nullDictionary;

            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            invalidArgumentTemplateProcessingException.AddData(
                key: "replacementDictionary",
                values: "Dictionary values is required");

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(It.IsAny<string>(), inputTemplate.ReplacementDictionary),
                    Times.Never);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
