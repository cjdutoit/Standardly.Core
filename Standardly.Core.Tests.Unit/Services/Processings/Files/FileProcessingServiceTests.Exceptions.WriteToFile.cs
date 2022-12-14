// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowDependencyValidationOnWriteToFileAsyncIfDependencyValidationErrorOccursAndLogIt(
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
                service.CheckIfDirectoryExists(It.IsAny<string>()))
                    .Throws(dependencyValidationException);

            // when
            Action writeToFileAction = () =>
                this.fileProcessingService.WriteToFile(inputPath, inputContent);

            // then
            FileProcessingDependencyValidationException actualException =
                Assert.Throws<FileProcessingDependencyValidationException>(writeToFileAction);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFile(inputPath, inputContent),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyOnWriteToFileAsyncIfDependencyErrorOccursAndLogIt(
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
                service.CheckIfDirectoryExists(It.IsAny<string>()))
                    .Throws(dependencyException);

            // when
            Action writeToFileAction = () =>
                this.fileProcessingService.WriteToFile(inputPath, inputContent);

            // then
            FileProcessingDependencyException actualException =
                Assert.Throws<FileProcessingDependencyException>(writeToFileAction);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFile(inputPath, inputContent),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnWriteToFileAsyncIfServiceErrorOccursAndLogIt()
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
                service.CheckIfDirectoryExists(It.IsAny<string>()))
                    .Throws(serviceException);

            this.fileServiceMock.Setup(service =>
                service.WriteToFile(It.IsAny<string>(), inputContent))
                    .Throws(serviceException);

            // when
            Action writeToFileAction = () =>
                this.fileProcessingService.WriteToFile(inputPath, inputContent);

            // then
            FileProcessingServiceException actualException =
                Assert.Throws<FileProcessingServiceException>(writeToFileAction);

            this.fileServiceMock.Verify(service =>
            service.CheckIfDirectoryExists(It.IsAny<string>()),
                Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFile(inputPath, inputContent),
                    Times.Never);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
