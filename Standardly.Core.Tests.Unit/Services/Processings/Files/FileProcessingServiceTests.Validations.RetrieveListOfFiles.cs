// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveListOfFilesIfInputsIsInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            string invalidSearchPattern = invalidInput;

            var invalidFilesProcessingException =
                new InvalidFileProcessingException();

            invalidFilesProcessingException.AddData(
                key: "path",
                values: "Text is required");

            invalidFilesProcessingException.AddData(
                key: "searchPattern",
                values: "Text is required");

            var expectedFilesProcessingValidationException =
                new FileProcessingValidationException(invalidFilesProcessingException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileProcessingService
                    .RetrieveListOfFilesAsync(path: invalidPath, searchPattern: invalidSearchPattern);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFilesProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(invalidPath, invalidSearchPattern),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
