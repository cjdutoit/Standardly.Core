// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
            ShouldThrowDependencyValidationOnDeleteDirectoryIfDependencyValidationErrorOccursAsync(
                Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var expectedOperationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.operationOrchestrationService.DeleteDirectoryAsync(inputPath, recursive);

            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(
                    deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationDependencyValidationException);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileDependencyExceptions))]
        public async Task ShouldThrowDependencyOnDeleteDirectoryIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var expectedOperationOrchestrationDependencyException =
                new OperationOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.operationOrchestrationService.DeleteDirectoryAsync(inputPath, recursive);

            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationDependencyException);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteDirectoryIfServiceErrorOccursAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;
            bool recursive = true;

            var serviceException = new Exception();

            var failedOperationOrchestrationServiceException =
                new FailedOperationOrchestrationServiceException(serviceException);

            var expectedOperationOrchestrationServiveException =
                new OperationOrchestrationServiceException(
                    failedOperationOrchestrationServiceException);

            this.fileProcessingServiceMock.Setup(service =>
                service.DeleteDirectoryAsync(inputPath, recursive))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> deleteDirectoryTask =
                this.operationOrchestrationService.DeleteDirectoryAsync(inputPath, recursive);

            OperationOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationServiceException>(deleteDirectoryTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationServiveException);

            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputPath, recursive),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
