// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Theory]
        [MemberData(nameof(FileServiceDependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnDeleteFileIfDependencyValidationErrorOccursAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            string somePath = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyValidationException);

            var expectedFileDependencyValidationException =
                new FileDependencyValidationException(invalidFileServiceDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFile(somePath))
                    .Throws(dependencyValidationException);

            // when
            Action deleteFileAction = () =>
                this.fileService.DeleteFile(somePath);

            FileDependencyValidationException actualException =
                Assert.Throws<FileDependencyValidationException>(deleteFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteFile(somePath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileDependencyValidationException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnDeleteFileIfDependencyErrorOccursAndLogIt(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFile(somePath))
                    .Throws(dependencyException);

            // when
            Action deleteFileAction = () =>
                this.fileService.DeleteFile(somePath);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(deleteFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteFile(somePath),
                    Times.AtLeastOnce);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformation(It.IsAny<string>()),
                    Times.Between(0, 3, Moq.Range.Inclusive));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileDependencyException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalFileDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnDeleteFileIfDependencyErrorOccursAndLogItCritical(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFile(somePath))
                    .Throws(dependencyException);

            // when
            Action deleteFileAction = () =>
                this.fileService.DeleteFile(somePath);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(deleteFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteFile(somePath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFileDependencyException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnDeleteFileIfServiceErrorOccurs()
        {
            // given
            string somePath = GetRandomString();
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(serviceException);

            var expectedFileServiceException =
                new FileServiceException(failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFile(somePath))
                    .Throws(serviceException);

            // when
            Action deleteFileAction = () =>
                this.fileService.DeleteFile(somePath);

            FileServiceException actualException =
                Assert.Throws<FileServiceException>(deleteFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteFile(somePath),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileServiceException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}