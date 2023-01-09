// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Processings.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnWriteToFileIfInputsIsInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            string invalidContent = invalidInput;

            var invalidFileProcessingException =
                new InvalidFileProcessingException();

            invalidFileProcessingException.AddData(
                key: "path",
                values: "Text is required");

            invalidFileProcessingException.AddData(
                key: "content",
                values: "Text is required");

            var expectedFileProcessingValidationException =
                new FileProcessingValidationException(invalidFileProcessingException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileProcessingService.WriteToFileAsync(path: invalidPath, content: invalidContent);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(invalidPath, invalidContent),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
