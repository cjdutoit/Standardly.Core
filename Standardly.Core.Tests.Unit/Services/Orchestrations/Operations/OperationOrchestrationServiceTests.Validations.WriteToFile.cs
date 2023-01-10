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
        public async Task ShouldThrowValidationExceptionOnWriteToFileIfInputsIsInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            string invalidContent = invalidInput;

            var invalidArgumentOperationOrchestrationException =
                new InvalidArgumentOperationOrchestrationException();

            invalidArgumentOperationOrchestrationException.AddData(
                key: "path",
                values: "Text is required");

            invalidArgumentOperationOrchestrationException.AddData(
                key: "content",
                values: "Text is required");

            var expectedOperationOrchestrationValidationException =
                new OperationOrchestrationValidationException(invalidArgumentOperationOrchestrationException);

            // when
            ValueTask<bool> writeToFileTask =
                this.operationOrchestrationService.WriteToFileAsync(path: invalidPath, content: invalidContent);

            OperationOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationValidationException>(writeToFileTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationValidationException);

            this.fileProcessingServiceMock.Verify(service =>
                service.WriteToFileAsync(invalidPath, invalidContent),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
