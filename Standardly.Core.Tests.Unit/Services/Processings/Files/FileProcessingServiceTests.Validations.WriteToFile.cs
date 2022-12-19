// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnWriteToFileIfInputsIsInvalidAndLogIt(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            string invalidContent = invalidInput;

            var invalidFilesProcessingException =
                new InvalidFileProcessingException();

            invalidFilesProcessingException.AddData(
                key: "path",
                values: "Text is required");

            invalidFilesProcessingException.AddData(
                key: "content",
                values: "Text is required");

            var expectedFilesProcessingValidationException =
                new FileProcessingValidationException(invalidFilesProcessingException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileProcessingService.WriteToFileAsync(path: invalidPath, content: invalidContent);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFilesProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(invalidPath, invalidContent),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
