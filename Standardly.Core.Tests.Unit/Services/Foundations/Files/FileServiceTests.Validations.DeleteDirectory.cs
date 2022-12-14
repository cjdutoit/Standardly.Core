// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
        public void ShouldThrowValidationExceptionOnDeleteDirectoryIfArgumantsIsInvalid(string invalidValue)
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
            System.Action writeToFileAction = () =>
                this.fileService.DeleteDirectory(invalidPath, recursive);

            FileValidationException actualException =
                Assert.Throws<FileValidationException>(writeToFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(invalidPath, recursive),
                        Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
