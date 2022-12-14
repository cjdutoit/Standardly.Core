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

            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(
                    retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationDependencyValidationException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
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

            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
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

            OperationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationServiceException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationServiveException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputPath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
