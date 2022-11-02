// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task ShouldThrowValidationExceptionOnTransformStringIfContentIsNullOrEmpty(string invalidString)
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
            ValueTask<string> transformStringTask =
                this.templateService.TransformString(content, inputReplacementDictionary);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(transformStringTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnTransformStringIfDictionaryIsNull()
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
            ValueTask<string> transformStringTask =
                this.templateService.TransformString(content, inputReplacementDictionary);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(transformStringTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
