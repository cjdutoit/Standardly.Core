// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Processings.Files.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnDeleteDirectoryIfDependencyValidationErrorOccursAsync(
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
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.fileProcessingService.DeleteDirectoryAsync(inputPath, recursive);

            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyValidationException);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnDeleteDirectoryIfDependencyErrorOccursAsync(
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
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.fileProcessingService.DeleteDirectoryAsync(inputPath, recursive);

            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyException);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteDirectoryIfServiceErrorOccursAsync()
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
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.fileProcessingService.DeleteDirectoryAsync(inputPath, recursive);

            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingServiveException);

            this.fileServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
