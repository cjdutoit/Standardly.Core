// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Files.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Theory]
        [MemberData(nameof(FileServiceDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnCheckIfDirectoryExistsIfDependencyValidationErrorOccursAsync(
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
                broker.CheckIfDirectoryExistsAsync(somePath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileService.CheckIfDirectoryExistsAsync(somePath);

            FileDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileDependencyValidationException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.CheckIfDirectoryExistsAsync(somePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnCheckIfDirectoryExistsIfDependencyErrorOccursAsync(
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
                broker.CheckIfDirectoryExistsAsync(somePath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileService.CheckIfDirectoryExistsAsync(somePath);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.CheckIfDirectoryExistsAsync(somePath),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnCheckIfDirectoryExistsIfServiceErrorOccursAsync()
        {
            // given
            string somePath = GetRandomString();
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(serviceException);

            var expectedFileServiceException =
                new FileServiceException(failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.CheckIfDirectoryExistsAsync(somePath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileService.CheckIfDirectoryExistsAsync(somePath);

            FileServiceException actualException =
                await Assert.ThrowsAsync<FileServiceException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.CheckIfDirectoryExistsAsync(somePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}