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

            var invalidFileProcessingException =
                new InvalidFileProcessingException();

            invalidFileProcessingException.AddData(
                key: "path",
                values: "Text is required");

            invalidFileProcessingException.AddData(
                key: "searchPattern",
                values: "Text is required");

            var expectedFileProcessingValidationException =
                new FileProcessingValidationException(invalidFileProcessingException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileProcessingService
                    .RetrieveListOfFilesAsync(path: invalidPath, searchPattern: invalidSearchPattern);

            FileProcessingValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(invalidPath, invalidSearchPattern),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
