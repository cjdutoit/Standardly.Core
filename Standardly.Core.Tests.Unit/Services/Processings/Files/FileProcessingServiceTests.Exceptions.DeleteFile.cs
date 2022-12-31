﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationOnDeleteFileAsyncIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> deleteFileTask =
                this.fileProcessingService.DeleteFileAsync(inputPath);

            // then
            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(deleteFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnDeleteFileAsyncIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> deleteFileTask =
                this.fileProcessingService.DeleteFileAsync(inputPath);

            // then
            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(deleteFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteFileAsyncIfServiceErrorOccursAndLogIt()
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
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> deleteFileTask =
                this.fileProcessingService.DeleteFileAsync(inputPath);

            // then
            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(deleteFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
