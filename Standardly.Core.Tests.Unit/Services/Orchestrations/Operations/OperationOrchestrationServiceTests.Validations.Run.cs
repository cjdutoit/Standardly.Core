// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Executions;
using Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionsIsNullAsync()
        {
            // given
            List<Execution> nullExecutions = null;
            string executionFolder = GetRandomString();

            var invalidArgumentOperationOrchestrationException =
                new InvalidArgumentOperationOrchestrationException();

            invalidArgumentOperationOrchestrationException.AddData(
                key: "executions",
                values: "Executions is required");

            var expectedOperationOrchestrationValidationException =
                new OperationOrchestrationValidationException(invalidArgumentOperationOrchestrationException);

            // when
            ValueTask<string> runTask =
                this.operationOrchestrationService.RunAsync(nullExecutions, executionFolder);

            OperationOrchestrationValidationException actualOperationOrchestrationValidationException =
                await Assert.ThrowsAsync<OperationOrchestrationValidationException>(runTask.AsTask);

            // then
            actualOperationOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedOperationOrchestrationValidationException);

            this.executionProcessingServiceMock.Verify(service =>
                service.RunAsync(nullExecutions, executionFolder),
                    Times.Never);

            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionFolderIsInvalidAsync(
            string invalidValue)
        {
            // given
            List<Execution> nullExecutions = GetRandomExecutions();
            string executionFolder = invalidValue;

            var invalidArgumentOperationOrchestrationException =
                new InvalidArgumentOperationOrchestrationException();

            invalidArgumentOperationOrchestrationException.AddData(
                key: "executionFolder",
                values: "Text is required");

            var expectedOperationOrchestrationValidationException =
                new OperationOrchestrationValidationException(invalidArgumentOperationOrchestrationException);

            // when
            ValueTask<string> runTask =
                this.operationOrchestrationService.RunAsync(nullExecutions, executionFolder);

            OperationOrchestrationValidationException actualOperationOrchestrationValidationException =
                await Assert.ThrowsAsync<OperationOrchestrationValidationException>(runTask.AsTask);

            // then
            actualOperationOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedOperationOrchestrationValidationException);

            this.executionProcessingServiceMock.Verify(service =>
                service.RunAsync(nullExecutions, executionFolder),
                    Times.Never);

            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
