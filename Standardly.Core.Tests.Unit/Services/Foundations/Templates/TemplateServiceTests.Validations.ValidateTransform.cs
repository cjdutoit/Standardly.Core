// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ShouldThrowValidationExceptionOnValidateTransformIfStringArgumentsInvalid(string invalidString)
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
            Action validateTransformationAction = () =>
                this.templateService.ValidateTransformation(invalidContent);

            TemplateValidationException actualTemplateValidationException =
                Assert.Throws<TemplateValidationException>(validateTransformationAction);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Fact]
        public void ShouldThrowValidationExceptionOnValidateTransformIfAllTagsNotReplaced()
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
            Action validateTransformationAction = () =>
                this.templateService.ValidateTransformation(inputStringTemplate);

            TemplateValidationException actualTemplateValidationException =
                Assert.Throws<TemplateValidationException>(validateTransformationAction);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
