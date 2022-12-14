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
            ShouldThrowDependencyValidationOnReadFromFileAsyncIfDependencyValidationErrorOccursAsync(
                Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputPath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> ReadFromFileTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(ReadFromFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyValidationException);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnReadFromFileAsyncIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(inputPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> ReadFromFileTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(ReadFromFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyException);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnReadFromFileAsyncIfServiceErrorOccursAsync()
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
                service.ReadFromFileAsync(inputPath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> ReadFromFileTask =
                this.fileProcessingService.ReadFromFileAsync(inputPath);

            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(ReadFromFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingServiveException);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
