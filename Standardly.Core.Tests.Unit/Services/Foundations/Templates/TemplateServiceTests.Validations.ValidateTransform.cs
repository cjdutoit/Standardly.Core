// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnValidateTransformIfStringArgumentsInvalidAsync(
            string invalidString)
        {
            // given
            string invalidContent = invalidString;

            var invalidArgumentTemplateException =
               new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "content",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            ValueTask validateTransformationTask =
                this.templateService.ValidateTransformationAsync(invalidContent);

            TemplateValidationException actualTemplateValidationException =
                await Assert.ThrowsAsync<TemplateValidationException>(validateTransformationTask.AsTask);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnValidateTransformIfAllTagsNotReplacedAsync()
        {
            // given
            string notReplacedTag = "$notReplaced$";
            string randomStringTemplate = $"{GetRandomString()}{notReplacedTag}";
            string inputStringTemplate = randomStringTemplate;

            var invalidReplacementException =
                new InvalidReplacementTemplateException();

            invalidReplacementException.AddData(
                key: "$notReplaced$",
                values: $"Found tag '{notReplacedTag}' that was not in the replacement dictionary.");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidReplacementException);

            // when
            ValueTask validateTransformationTask =
                this.templateService.ValidateTransformationAsync(inputStringTemplate);

            TemplateValidationException actualTemplateValidationException =
                await Assert.ThrowsAsync<TemplateValidationException>(validateTransformationTask.AsTask);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
