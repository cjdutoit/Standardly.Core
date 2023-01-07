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
    }
}
