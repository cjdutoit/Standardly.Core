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
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnAppendContentIfArgsInvalidAndLog(string invalidString)
        {
            // given
            string sourceContent = invalidString;
            string regexToMatch = invalidString;
            string appendContent = invalidString;
            string doesNotContainContent = invalidString;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var invalidArgumentTemplateException =
                            new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "sourceContent",
                values: "Text is required");

            invalidArgumentTemplateException.AddData(
                key: "regexToMatch",
                values: "Text is required");

            invalidArgumentTemplateException.AddData(
                key: "appendContent",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            Action appendContentAction = () =>
                this.templateService.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            var actualTemplateValidationException =
                Assert.Throws<TemplateValidationException>(appendContentAction);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Fact]
        public void ShouldThrowValidationExceptionOnAppendContentIfWithNoMatchFoundAndLog()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatch = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var regularExpressionTemplateException
                = new RegularExpressionTemplateException("No match found. Please verify the expression and source");

            var expectedTemplateValidationException =
                new TemplateValidationException(regularExpressionTemplateException);

            // when
            Action appendContentAction = () =>
                this.templateService.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            var actualTemplateValidationException =
                Assert.Throws<TemplateValidationException>(appendContentAction);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
