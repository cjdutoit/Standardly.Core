// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
            ShouldThrowDependencyValidationOnRetrieveListOfFilesAsyncIfDependencyValidationErrorOccursAsync(
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
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.operationOrchestrationService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(retrieveListOfFilesTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileDependencyExceptions))]
        public async Task ShouldThrowDependencyOnRetrieveListOfFilesAsyncIfDependencyErrorOccursAsync(
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
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.operationOrchestrationService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(retrieveListOfFilesTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveListOfFilesAsyncIfServiceErrorOccursAsync()
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
                service.RetrieveListOfFilesAsync(inputPath, inputContent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.operationOrchestrationService.RetrieveListOfFilesAsync(inputPath, inputContent);

            // then
            OperationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationServiceException>(retrieveListOfFilesTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
