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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveListOfFilesIfDependencyValidationErrorOccursAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyValidationException);

            var expectedFileDependencyValidationException =
                new FileDependencyValidationException(invalidFileServiceDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern))
                    .Throws(dependencyValidationException);

            // when
            Action retrieveListOfFilesAction = () =>
                this.fileService.RetrieveListOfFiles(somePath, someSearchPattern);

            FileDependencyValidationException actualException =
                Assert.Throws<FileDependencyValidationException>(retrieveListOfFilesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveListOfFilesIfDependencyErrorOccursAndLogIt(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern))
                    .Throws(dependencyException);

            // when
            Action retrieveListOfFilesAction = () =>
                this.fileService.RetrieveListOfFiles(somePath, someSearchPattern);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(retrieveListOfFilesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalFileDependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveListOfFilesIfDependencyErrorOccursAndLogItCritical(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern))
                    .Throws(dependencyException);

            // when
            Action retrieveListOfFilesAction = () =>
                this.fileService.RetrieveListOfFiles(somePath, someSearchPattern);

            FileDependencyException actualException =
                Assert.Throws<FileDependencyException>(retrieveListOfFilesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnRetrieveListOfFilesIfServiceErrorOccurs()
        {
            // given
            string somePath = GetRandomString();
            string someSearchPattern = GetRandomString();
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(serviceException);

            var expectedFileServiceException =
                new FileServiceException(failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern))
                    .Throws(serviceException);

            // when
            Action retrieveListOfFilesAction = () =>
                this.fileService.RetrieveListOfFiles(somePath, someSearchPattern);

            FileServiceException actualException =
                Assert.Throws<FileServiceException>(retrieveListOfFilesAction);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFiles(somePath, someSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}