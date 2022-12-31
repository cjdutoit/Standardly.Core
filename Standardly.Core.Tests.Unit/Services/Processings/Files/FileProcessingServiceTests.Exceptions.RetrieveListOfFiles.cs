// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveListOfFilesAsyncIfDependencyValidationErrorOccursAndLogIt(
                Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileProcessingService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(retrieveListOfFilesTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRetrieveListOfFilesAsyncIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileProcessingService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(retrieveListOfFilesTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveListOfFilesAsyncIfServiceErrorOccursAndLogIt()
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var serviceException = new Exception();

            var failedFileProcessingServiceException =
                new FailedFileProcessingServiceException(serviceException);

            var expectedFileProcessingServiveException =
                new FileProcessingServiceException(
                    failedFileProcessingServiceException);

            this.fileServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileProcessingService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(retrieveListOfFilesTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
