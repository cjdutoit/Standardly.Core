// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Executions
{
    public partial class ExecutionProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionsIsNullAndLogItAsync()
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
                this.executionProcessingService.Run(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                await Assert.ThrowsAsync<ExecutionProcessingValidationException>(runTask.AsTask);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionProcessingValidationException))),
                        Times.Once);

            this.executionServiceMock.Verify(broker =>
                broker.Run(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionFolderIsInvalidAndLogItAsync(string invalidValue)
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
                this.executionProcessingService.Run(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                await Assert.ThrowsAsync<ExecutionProcessingValidationException>(runTask.AsTask);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionProcessingValidationException))),
                        Times.Once);

            this.executionServiceMock.Verify(broker =>
                broker.Run(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
