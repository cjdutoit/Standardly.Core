// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveListOfFilesIfDependencyValidationErrorOccursAndLogItAsync(
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
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileDependencyValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyValidationException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveListOfFilesIfDependencyErrorOccursAndLogItAsync(
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
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFileDependencyException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalFileDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveListOfFilesIfDependencyErrorOccursAndLogItCriticalAsync(
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
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileDependencyException actualException =
                await Assert.ThrowsAsync<FileDependencyException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileDependencyException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFileDependencyException))),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnRetrieveListOfFilesIfServiceErrorOccursAsync()
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
                broker.GetListOfFilesAsync(somePath, someSearchPattern))
                    .Throws(serviceException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.fileService.RetrieveListOfFilesAsync(somePath, someSearchPattern);

            FileServiceException actualException =
                await Assert.ThrowsAsync<FileServiceException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileServiceException);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFilesAsync(somePath, someSearchPattern),
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