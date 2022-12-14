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
        public void ShouldThrowDependencyValidationOnDeleteDirectoryIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.DeleteDirectory(inputPath, recursive))
                    .Throws(dependencyValidationException);

            // when
            Action deleteDirectoryAction = () =>
                this.fileProcessingService.DeleteDirectory(inputPath, recursive);

            // then
            FileProcessingDependencyValidationException actualException =
                Assert.Throws<FileProcessingDependencyValidationException>(deleteDirectoryAction);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectory(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyOnDeleteDirectoryIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.DeleteDirectory(inputPath, recursive))
                    .Throws(dependencyException);

            // when
            Action deleteDirectoryAction = () =>
                this.fileProcessingService.DeleteDirectory(inputPath, recursive);

            // then
            FileProcessingDependencyException actualException =
                Assert.Throws<FileProcessingDependencyException>(deleteDirectoryAction);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectory(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnDeleteDirectoryIfServiceErrorOccursAndLogIt()
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var serviceException = new Exception();

            var failedFileProcessingServiceException =
                new FailedFileProcessingServiceException(serviceException);

            var expectedFileProcessingServiveException =
                new FileProcessingServiceException(
                    failedFileProcessingServiceException);

            this.fileServiceMock.Setup(service =>
                service.DeleteDirectory(inputPath, recursive))
                    .Throws(serviceException);

            // when
            Action deleteDirectoryAction = () =>
                this.fileProcessingService.DeleteDirectory(inputPath, recursive);

            // then
            FileProcessingServiceException actualException =
                Assert.Throws<FileProcessingServiceException>(deleteDirectoryAction);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectory(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
