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
        public async Task ShouldThrowDependencyValidationOnWriteToFileAsyncIfDependencyValidationErrorOccursAndLogItAsync(
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
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .Throws(dependencyValidationException);

            // when
            ValueTask writeToFileTask =
                this.fileProcessingService.WriteToFileAsync(inputPath, inputContent);

            // then
            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(writeToFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingDependencyValidationException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnWriteToFileAsyncIfDependencyErrorOccursAndLogItAsync(
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
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .Throws(dependencyException);

            // when
            ValueTask writeToFileTask =
                this.fileProcessingService.WriteToFileAsync(inputPath, inputContent);

            // then
            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(writeToFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingDependencyException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnWriteToFileAsyncIfServiceErrorOccursAndLogItAsync()
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
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .Throws(serviceException);

            this.fileServiceMock.Setup(service =>
                service.WriteToFileAsync(It.IsAny<string>(), inputContent))
                    .Throws(serviceException);

            // when
            ValueTask writeToFileTask =
                this.fileProcessingService.WriteToFileAsync(inputPath, inputContent);

            // then
            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(writeToFileTask.AsTask);

            this.fileServiceMock.Verify(service =>
            service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingServiveException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
