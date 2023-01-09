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
using Standardly.Core.Models.Services.Processings.Executions.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Executions
{
    public partial class ExecutionProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionsIsNullAsync()
        {
            // given
            List<Execution> nullExecutions = null;
            string executionFolder = GetRandomString();

            var invalidArgumentExecutionProcessingException =
                new InvalidArgumentExecutionProcessingException();

            invalidArgumentExecutionProcessingException.AddData(
                key: "executions",
                values: "Executions is required");

            var expectedExecutionProcessingValidationException =
                new ExecutionProcessingValidationException(invalidArgumentExecutionProcessingException);

            // when
            ValueTask<string> runTask =
                this.executionProcessingService.RunAsync(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                await Assert.ThrowsAsync<ExecutionProcessingValidationException>(runTask.AsTask);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.executionServiceMock.Verify(service =>
                service.RunAsync(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionFolderIsInvalidAsync(string invalidValue)
        {
            // given
            List<Execution> nullExecutions = GetRandomExecutions();
            string executionFolder = invalidValue;

            var invalidArgumentExecutionProcessingException =
                new InvalidArgumentExecutionProcessingException();

            invalidArgumentExecutionProcessingException.AddData(
                key: "executionFolder",
                values: "Text is required");

            var expectedExecutionProcessingValidationException =
                new ExecutionProcessingValidationException(invalidArgumentExecutionProcessingException);

            // when
            ValueTask<string> runTask =
                this.executionProcessingService.RunAsync(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                await Assert.ThrowsAsync<ExecutionProcessingValidationException>(runTask.AsTask);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.executionServiceMock.Verify(service =>
                service.RunAsync(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
        }
    }
}
