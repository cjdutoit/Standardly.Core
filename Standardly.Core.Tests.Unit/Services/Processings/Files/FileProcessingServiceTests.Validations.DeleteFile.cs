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
        public async Task ShouldThrowValidationExceptionOnDeleteFileIfInputsIsInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;

            var invalidFileProcessingException =
                new InvalidFileProcessingException();

            invalidFileProcessingException.AddData(
                key: "path",
                values: "Text is required");

            var expectedFileProcessingValidationException =
                new FileProcessingValidationException(invalidFileProcessingException);

            // when
            ValueTask<bool> deleteFileTask =
                this.fileProcessingService.DeleteFileAsync(path: invalidPath);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(deleteFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.DeleteFileAsync(invalidPath),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
