// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateDirectoryIfArgumentsIsInvalidAsync(string invalidValue)
        {
            // given
            string invalidPath = invalidValue;

            var invalidArgumentFileException =
                new InvalidArgumentFileException();

            invalidArgumentFileException.AddData(
                key: "path",
                values: "Text is required");

            var expectedFileValidationException =
                new FileValidationException(invalidArgumentFileException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.CreateDirectoryAsync(invalidPath);

            FileValidationException actualException =
                await Assert.ThrowsAsync<FileValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.CreateDirectoryAsync(invalidPath),
                        Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}