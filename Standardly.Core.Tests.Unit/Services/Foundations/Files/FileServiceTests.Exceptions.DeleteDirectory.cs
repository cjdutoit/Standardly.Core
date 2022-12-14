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
        public void ShouldThrowDependencyValidationExceptionOnDeleteDirectoryIfDependencyValidationErrorOccursAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            string somePath = GetRandomString();
            bool recursive = true;

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyValidationException);

            var expectedFileDependencyValidationException =
                new FileDependencyValidationException(invalidFileServiceDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteDirectory(somePath, recursive))
                    .Throws(dependencyValidationException);

            // when
            Action writeToFileAction = () =>
                this.fileService.DeleteDirectory(somePath, recursive);

            FileDependencyValidationException actualException =
                Assert.Throws<FileDependencyValidationException>(writeToFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnDeleteDirectoryIfDependencyErrorOccursAndLogIt(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            bool recursive = true;

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteDirectory(somePath, recursive))
                    .Throws(dependencyException);

            // when
            Action writeToFileAction = () =>
                this.fileService.DeleteDirectory(somePath, recursive);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(writeToFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(somePath, recursive),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalFileDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnDeleteDirectoryIfDependencyErrorOccursAndLogItCritical(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            bool recursive = true;

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteDirectory(somePath, recursive))
                    .Throws(dependencyException);

            // when
            Action writeToFileAction = () =>
                this.fileService.DeleteDirectory(somePath, recursive);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(writeToFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnDeleteDirectoryIfServiceErrorOccurs()
        {
            // given
            string somePath = GetRandomString();
            bool recursive = true;
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(serviceException);

            var expectedFileServiceException =
                new FileServiceException(failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteDirectory(somePath, recursive))
                    .Throws(serviceException);

            // when
            Action writeToFileAction = () =>
                this.fileService.DeleteDirectory(somePath, recursive);

            FileServiceException actualException =
                Assert.Throws<FileServiceException>(writeToFileAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}