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
using Standardly.Core.Models.Foundations.Executions.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Executions
{
    public partial class ExecutionServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionFolderIsInvalid(string invalidValue)
        {
            // given
            string inputExecutionFolder = invalidValue;

            List<Execution> someExecutions = GetRandomExecutions();

            var invalidArgumentExecutionException =
                new InvalidArgumentExecutionException();

            invalidArgumentExecutionException.AddData(
                key: "executionFolder",
                values: "Text is required");

            var expectedExecutionValidationException =
                new ExecutionValidationException(invalidArgumentExecutionException);

            // when
            ValueTask<string> runTask = this.executionService.RunAsync(someExecutions, inputExecutionFolder);

            ExecutionValidationException actualExecutionValidationException =
                await Assert.ThrowsAsync<ExecutionValidationException>(runTask.AsTask);

            // then
            actualExecutionValidationException.Should().BeEquivalentTo(expectedExecutionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionValidationException))),
                        Times.Once);

            this.executionBrokerMock.Verify(broker =>
                broker.Run(someExecutions, inputExecutionFolder),
                    Times.Never);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidExecutions))]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionsIsNull(List<Execution> invalidExecutions)
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> inputExecutions = invalidExecutions;

            var invalidArgumentExecutionException =
                new InvalidArgumentExecutionException();

            invalidArgumentExecutionException.AddData(
                key: "executions",
                values: "Executions is required");

            var expectedExecutionValidationException =
                new ExecutionValidationException(invalidArgumentExecutionException);

            // when
            ValueTask<string> runTask = this.executionService.RunAsync(inputExecutions, inputExecutionFolder);

            ExecutionValidationException actualExecutionValidationException =
                await Assert.ThrowsAsync<ExecutionValidationException>(runTask.AsTask);

            // then
            actualExecutionValidationException.Should().BeEquivalentTo(expectedExecutionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionValidationException))),
                        Times.Once);

            this.executionBrokerMock.Verify(broker =>
                broker.Run(inputExecutions, inputExecutionFolder),
                    Times.Never);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
