// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveListOfFilesIfPathIsInvalid(string invalidValue)
        {
            // given
            string invalidPath = invalidValue;
            string invalidSearchPattern = invalidValue;

            var invalidArgumentFileException =
                new InvalidArgumentFileException();

            invalidArgumentFileException.AddData(
                key: "path",
                values: "Text is required");

            invalidArgumentFileException.AddData(
                key: "searchPattern",
                values: "Text is required");

            var expectedFileValidationException =
                new FileValidationException(invalidArgumentFileException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(invalidPath, invalidSearchPattern);

            FileValidationException actualException =
                await Assert.ThrowsAsync<FileValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(invalidPath, invalidSearchPattern),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
