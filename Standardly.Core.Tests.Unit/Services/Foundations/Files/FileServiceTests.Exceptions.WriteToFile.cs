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
        public async Task ShouldThrowDependencyValidationExceptionOnWriteToFileIfDependencyValidationErrorOccursAsync(
            Exception dependencyValidationException)
        {
            // given
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyValidationException);

            var expectedFileDependencyValidationException =
                new FileDependencyValidationException(invalidFileServiceDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.WriteToFileAsync(somePath, someContent))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.WriteToFileAsync(somePath, someContent);

            FileDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileDependencyValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.WriteToFileAsync(somePath, someContent),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileServiceDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnWriteToFileIfDependencyErrorOccursAsync(
            Exception dependencyException)
        {
            // given
            string somePath = GetRandomString();
            string someContent = GetRandomString();

            var invalidFileServiceDependencyException =
                new InvalidFileServiceDependencyException(
                    dependencyException);

            var failedFileDependencyException =
                new FailedFileDependencyException(
                    invalidFileServiceDependencyException);

            var expectedFileDependencyException =
                new FileDependencyException(failedFileDependencyException);

            this.fileBrokerMock.Setup(broker =>
                broker.WriteToFileAsync(somePath, someContent))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.WriteToFileAsync(somePath, someContent);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.WriteToFileAsync(somePath, someContent),
                    Times.AtLeastOnce);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnWriteToFileIfServiceErrorOccursAsync()
        {
            // given
            string somePath = GetRandomString();
            string someContent = GetRandomString();
            var serviceException = new Exception();

            var failedFileServiceException =
                new FailedFileServiceException(serviceException);

            var expectedFileServiceException =
                new FileServiceException(failedFileServiceException);

            this.fileBrokerMock.Setup(broker =>
                broker.WriteToFileAsync(somePath, someContent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> writeToFileTask =
                this.fileService.WriteToFileAsync(somePath, someContent);

            FileServiceException actualException =
                await Assert.ThrowsAsync<FileServiceException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.WriteToFileAsync(somePath, someContent),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}