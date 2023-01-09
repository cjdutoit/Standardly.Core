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
        public async Task ShouldThrowValidationExceptionOnCreateDirectoryIfInputsIsInvalidAsync(
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
            ValueTask<bool> createDirectoryTask =
                this.fileProcessingService.CreateDirectoryAsync(path: invalidPath);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(createDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.CreateDirectoryAsync(invalidPath),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
