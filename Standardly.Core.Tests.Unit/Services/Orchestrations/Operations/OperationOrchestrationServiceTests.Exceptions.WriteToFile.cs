// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Orchestrations.Operations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(FileDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnWriteToFileAsyncIfDependencyValidationErrorOccursAsync(
                Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var expectedOperationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> writeToFileTask =
                this.operationOrchestrationService.WriteToFileAsync(inputPath, inputContent);

            // then
            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(writeToFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileDependencyExceptions))]
        public async Task ShouldThrowDependencyOnWriteToFileAsyncIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var expectedOperationOrchestrationDependencyException =
                new OperationOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> writeToFileTask =
                this.operationOrchestrationService.WriteToFileAsync(inputPath, inputContent);

            // then
            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(writeToFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnWriteToFileAsyncIfServiceErrorOccursAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            string inputContent = randomPath;

            var serviceException = new Exception();

            var failedOperationOrchestrationServiceException =
                new FailedOperationOrchestrationServiceException(serviceException);

            var expectedOperationOrchestrationServiveException =
                new OperationOrchestrationServiceException(
                    failedOperationOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            this.fileProcessingServiceMock.Setup(service =>
                service.WriteToFileAsync(It.IsAny<string>(), inputContent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> writeToFileTask =
                this.operationOrchestrationService.WriteToFileAsync(inputPath, inputContent);

            // then
            OperationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationServiceException>(writeToFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
            service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                Times.Once);

            this.fileProcessingServiceMock.Verify(service =>
                service.WriteToFileAsync(inputPath, inputContent),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
