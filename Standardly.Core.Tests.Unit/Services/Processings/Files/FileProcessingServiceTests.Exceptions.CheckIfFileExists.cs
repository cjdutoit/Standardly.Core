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
        public async Task ShouldThrowDependencyValidationOnCheckIfFileExistsIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(inputPath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileProcessingService.CheckIfFileExistsAsync(inputPath);

            FileProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyValidationException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyValidationException);

            this.fileServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnCheckIfFileExistsIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedFileProcessingDependencyException =
                new FileProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(inputPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileProcessingService.CheckIfFileExistsAsync(inputPath);

            FileProcessingDependencyException actualException =
                await Assert.ThrowsAsync<FileProcessingDependencyException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingDependencyException);

            this.fileServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCheckIfFileExistsIfServiceErrorOccursAsync()
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
                service.CheckIfFileExistsAsync(inputPath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.fileProcessingService.CheckIfFileExistsAsync(inputPath);

            FileProcessingServiceException actualException =
                await Assert.ThrowsAsync<FileProcessingServiceException>(checkIfFileExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedFileProcessingServiveException);

            this.fileServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputPath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
