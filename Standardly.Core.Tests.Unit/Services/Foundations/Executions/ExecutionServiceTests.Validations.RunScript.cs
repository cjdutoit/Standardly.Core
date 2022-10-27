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

            List<Execution> someExecutions = new List<Execution>()
            {
                new Execution(name: invalidValue, instruction: invalidValue)
            };

            var invalidArgumentExecutionException =
                new InvalidArgumentExecutionException();

            invalidArgumentExecutionException.AddData(
                key: "executionFolder",
                values: "Text is required");

            invalidArgumentExecutionException.AddData(
                key: "executions",
                values: "Executions is required");

            var expectedExecutionValidationException =
                new ExecutionValidationException(invalidArgumentExecutionException);

            // when
            ValueTask<string> runTask = this.executionService.Run(someExecutions, inputExecutionFolder);

            ExecutionValidationException actualExecutionValidationException =
                await Assert.ThrowsAsync<ExecutionValidationException>(runTask.AsTask);

            // then
            actualExecutionValidationException.Should().BeEquivalentTo(expectedExecutionValidationException);

            this.executionBrokerMock.Verify(broker =>
                broker.Run(someExecutions, inputExecutionFolder),
                    Times.Never);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRunIfExecutionsIsNull()
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> someExecutions = null;

            var invalidArgumentExecutionException =
                new InvalidArgumentExecutionException();

            invalidArgumentExecutionException.AddData(
                key: "executions",
                values: "Executions is required");

            var expectedExecutionValidationException =
                new ExecutionValidationException(invalidArgumentExecutionException);

            // when
            ValueTask<string> runTask = this.executionService.Run(someExecutions, inputExecutionFolder);

            ExecutionValidationException actualExecutionValidationException =
                await Assert.ThrowsAsync<ExecutionValidationException>(runTask.AsTask);

            // then
            actualExecutionValidationException.Should().BeEquivalentTo(expectedExecutionValidationException);

            this.executionBrokerMock.Verify(broker =>
                broker.Run(someExecutions, inputExecutionFolder),
                    Times.Never);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
