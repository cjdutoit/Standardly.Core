// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnDeleteDirectoryIfDependencyValidationErrorOccursAndLogIt(
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
                broker.DeleteDirectoryAsync(somePath, recursive))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.DeleteDirectoryAsync(somePath, recursive);

            FileDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileDependencyValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnDeleteDirectoryIfDependencyErrorOccursAndLogIt(
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
                broker.DeleteDirectoryAsync(somePath, recursive))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.DeleteDirectoryAsync(somePath, recursive);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(somePath, recursive),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalFileDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnDeleteDirectoryIfDependencyErrorOccursAndLogItCritical(
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
                broker.DeleteDirectoryAsync(somePath, recursive))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.DeleteDirectoryAsync(somePath, recursive);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnDeleteDirectoryIfServiceErrorOccurs()
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
                broker.DeleteDirectoryAsync(somePath, recursive))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.DeleteDirectoryAsync(somePath, recursive);

            FileServiceException actualException =
                await Assert.ThrowsAsync<FileServiceException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectoryAsync(somePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}