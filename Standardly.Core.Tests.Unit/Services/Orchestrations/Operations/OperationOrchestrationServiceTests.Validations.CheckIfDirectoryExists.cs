// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCheckIfDirectoryExistsIfPathIsInvalidAsync(
            string invalidFilePath)
        {
            // given
            var invalidArgumentOperationOrchestrationException =
                new InvalidArgumentOperationOrchestrationException();

            invalidArgumentOperationOrchestrationException.AddData(
                key: "path",
                values: "Text is required");

            var expectedOperationOrchestrationValidationException =
                new OperationOrchestrationValidationException(invalidArgumentOperationOrchestrationException);

            // when
            ValueTask<bool> checkIfDirectoryExistsTask =
                this.operationOrchestrationService.CheckIfDirectoryExistsAsync(invalidFilePath);

            OperationOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationValidationException>(checkIfDirectoryExistsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationValidationException);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(invalidFilePath),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
