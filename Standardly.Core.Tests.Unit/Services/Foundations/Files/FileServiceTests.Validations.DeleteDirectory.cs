// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnDeleteDirectoryIfArgumentsIsInvalidAsync(string invalidValue)
        {
            // given
            string invalidPath = invalidValue;
            bool recursive = true;

            var invalidArgumentFileException =
                new InvalidArgumentFileException();

            invalidArgumentFileException.AddData(
                key: "path",
                values: "Text is required");

            var expectedFileValidationException =
                new FileValidationException(invalidArgumentFileException);

            // when
            ValueTask<bool> writeToFileAction =
                this.fileService.DeleteDirectoryAsync(invalidPath, recursive);

            FileValidationException actualException =
                await Assert.ThrowsAsync<FileValidationException>(writeToFileAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(invalidPath, recursive),
                        Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
