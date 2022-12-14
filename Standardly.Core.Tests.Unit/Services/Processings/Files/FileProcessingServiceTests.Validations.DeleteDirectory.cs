// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowValidationExceptionOnDeleteDirectoryIfInputsIsInvalidAndLogIt(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            bool recursive = false;

            var invalidFilesProcessingException =
                new InvalidFileProcessingException();

            invalidFilesProcessingException.AddData(
                key: "path",
                values: "Text is required");

            var expectedFilesProcessingValidationException =
                new FileProcessingValidationException(invalidFilesProcessingException);

            // when
            Action deleteDirectoryAction = () =>
                this.fileProcessingService.DeleteDirectory(path: invalidPath, recursive);

            FileProcessingValidationException actualException =
                Assert.Throws<FileProcessingValidationException>(deleteDirectoryAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFilesProcessingValidationException);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectory(invalidPath, recursive),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
