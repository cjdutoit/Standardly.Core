// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowDependencyValidationOnCheckIfFileExistsIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedOperationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(inputPath))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.operationOrchestrationService.CheckIfFileExistsAsync(inputPath);

            // then
            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(
                    checkIfFileExistsTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputPath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(FileDependencyExceptions))]
        public async Task ShouldThrowDependencyOnCheckIfFileExistsIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomPath = GetRandomString();
            string inputPath = randomPath;

            var expectedOperationOrchestrationDependencyException =
                new OperationOrchestrationDependencyException(
                    dependencyException.InnerException as Xeption);

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(inputPath))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> checkIfFileExistsTask =
                this.operationOrchestrationService.CheckIfFileExistsAsync(inputPath);

            // then
            OperationOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyException>(checkIfFileExistsTask.AsTask);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputPath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
