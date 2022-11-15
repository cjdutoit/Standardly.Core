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
        public void ShouldThrowDependencyValidationOnCheckIfDirectoryExistsIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.CheckIfDirectoryExists(inputPath))
                    .Throws(dependencyValidationException);

            // when
            Action checkIfDirectoryExistsAction = () =>
                this.fileProcessingService.CheckIfDirectoryExists(inputPath);

            // then
            FileProcessingDependencyValidationException actualException =
                Assert.Throws<FileProcessingDependencyValidationException>(checkIfDirectoryExistsAction);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(inputPath),
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
        public void ShouldThrowDependencyOnCheckIfDirectoryExistsIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.CheckIfDirectoryExists(inputPath))
                    .Throws(dependencyException);

            // when
            Action checkIfDirectoryExistsAction = () =>
                this.fileProcessingService.CheckIfDirectoryExists(inputPath);

            // then
            FileProcessingDependencyException actualException =
                Assert.Throws<FileProcessingDependencyException>(checkIfDirectoryExistsAction);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(inputPath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileProcessingDependencyException))),
                        Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnCheckIfDirectoryExistsIfServiceErrorOccursAndLogIt()
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
                service.CheckIfDirectoryExists(inputPath))
                    .Throws(serviceException);

            // when
            Action checkIfDirectoryExistsTask = () =>
                this.fileProcessingService.CheckIfDirectoryExists(inputPath);

            // then
            FileProcessingServiceException actualException =
                Assert.Throws<FileProcessingServiceException>(checkIfDirectoryExistsTask);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(inputPath),
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
