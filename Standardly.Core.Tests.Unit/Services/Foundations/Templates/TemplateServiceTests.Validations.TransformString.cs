// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnTransformStringIfContentIsNullOrEmptyAsync(
            string invalidString)
        {
            // given
            string content = invalidString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var invalidArgumentTemplateException =
                            new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "content",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            ValueTask<string> transformStringAction =
                this.templateService.TransformStringAsync(content, inputReplacementDictionary);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(transformStringAction.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedTemplateValidationException);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnTransformStringIfDictionaryIsNullAsync()
        {
            // given
            string randomString = GetRandomString();
            string content = randomString;
            Dictionary<string, string> randomReplacementDictionary = null;
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var invalidArgumentTemplateException =
                            new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "replacementDictionary",
                values: "Dictionary is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            ValueTask<string> transformStringAction =
                this.templateService.TransformStringAsync(content, inputReplacementDictionary);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(transformStringAction.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedTemplateValidationException);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
