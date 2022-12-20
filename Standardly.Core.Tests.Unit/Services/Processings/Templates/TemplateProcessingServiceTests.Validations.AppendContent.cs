// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAppendContentIfArgumentsIsInvalidAndLogIt(
            string invalidInput)
        {
            // given
            string sourceContent = invalidInput;
            string regexToMatchForAppend = invalidInput;
            string appendContent = invalidInput;
            string doesNotContainContent = invalidInput;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            invalidArgumentTemplateProcessingException.AddData(
                key: "sourceContent",
                values: "Text is required");

            invalidArgumentTemplateProcessingException.AddData(
                key: "regexToMatch",
                values: "Text is required");

            invalidArgumentTemplateProcessingException.AddData(
                key: "appendContent",
                values: "Text is required");

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            ValueTask<string> appendContentAction =
                this.templateProcessingService
                    .AppendContentAsync(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(appendContentAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist),
                        Times.Never);

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
