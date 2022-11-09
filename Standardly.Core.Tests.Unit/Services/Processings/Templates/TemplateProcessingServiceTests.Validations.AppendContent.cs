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
        public async Task ShouldThrowValidationExceptionOnAppendContentIfArgumentsIsInvalidAndLogItAsync(
            string invalidInput)
        {
            // given
            string sourceContent = invalidInput;
            string regexToMatch = invalidInput;
            string appendContent = invalidInput;
            bool appendToBeginning = false;
            bool onlyAppendIfNotPresent = true;

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
            ValueTask<string> appendContentTask =
                this.templateProcessingService
                    .AppendContentAsync(
                        sourceContent,
                        regexToMatch,
                        appendContent,
                        appendToBeginning,
                        onlyAppendIfNotPresent);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(appendContentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingValidationException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.AppendContentAsync(
                    sourceContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    onlyAppendIfNotPresent),
                        Times.Never);

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
