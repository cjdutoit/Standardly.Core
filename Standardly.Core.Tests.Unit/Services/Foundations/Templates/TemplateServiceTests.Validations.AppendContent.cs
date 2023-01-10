// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAppendContentIfArgsInvalidAsync(string invalidString)
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
            ValueTask<string> appendContentTask =
                this.templateService.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            var actualTemplateValidationException =
                await Assert.ThrowsAsync<TemplateValidationException>(appendContentTask.AsTask);

            // then
            actualTemplateValidationException.Should()
                .BeEquivalentTo(expectedTemplateValidationException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Never);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAppendContentIfWithNoMatchFoundAsync()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatch = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var regularExpressionTemplateException
                = new RegularExpressionTemplateException(
                    "Regular expression match not found. Please verify the expression and source.  "
                    + $"Could not find a match for {regexToMatch} in {sourceContent}");

            var expectedTemplateValidationException =
                new TemplateValidationException(regularExpressionTemplateException);

            // when
            ValueTask<string> appendContentTask =
                this.templateService.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            var actualTemplateValidationException =
                await Assert.ThrowsAsync<TemplateValidationException>(appendContentTask.AsTask);

            // then
            actualTemplateValidationException.Should()
                .BeEquivalentTo(expectedTemplateValidationException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Once);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
