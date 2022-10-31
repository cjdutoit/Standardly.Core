// ---------------------------------------------------------------
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
        public async Task ShouldThrowDependencyValidationOnReadFromFileAsyncIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputPath))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<string> runTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            // then
            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(runTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingDependencyValidationException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnReadFromFileAsyncIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputPath))
                    .Throws(dependencyException);

            // when
            ValueTask<string> runTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            // then
            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(runTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingDependencyException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnReadFromFileAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var serviceException = new Exception();

            var failedFileProcessingServiceException =
                new FailedFileProcessingServiceException(serviceException);

            var expectedFileProcessingServiveException =
                new FileProcessingServiceException(
                    failedFileProcessingServiceException);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputPath))
                    .Throws(serviceException);

            // when
            ValueTask<string> runTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            // then
            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(runTask.AsTask);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingServiveException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
