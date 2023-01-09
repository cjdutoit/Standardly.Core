// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(FileDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnDeleteFileAsyncIfDependencyValidationErrorOccursAsync(
                Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedOperationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> deleteFileTask =
                this.operationOrchestrationService.DeleteFileAsync(inputPath);

            // then
            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(deleteFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileDependencyExceptions))]
        public async Task ShouldThrowDependencyOnDeleteFileAsyncIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedOperationOrchestrationDependencyException =
                new OperationOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> deleteFileTask =
                this.operationOrchestrationService.DeleteFileAsync(inputPath);

            // then
            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(deleteFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteFileAsyncIfServiceErrorOccursAsync()
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
                service.DeleteFileAsync(inputPath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> deleteFileTask =
                this.operationOrchestrationService.DeleteFileAsync(inputPath);

            // then
            OperationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationServiceException>(deleteFileTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteFileAsync(inputPath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
