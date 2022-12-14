// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Executions;
using Standardly.Core.Models.Services.Foundations.Executions.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Executions
{
    public partial class ExecutionServiceTests
    {
        [Fact]
        public async Task ShoudThrowServiceExceptionOnRunIfServiceErrorOccursAsync()
        {
            // given
            string somePath = GetRandomString();
            List<Execution> someExecutions = GetRandomExecutions();

            var serviceException = new Exception();

            var failedExecutionServiceException =
                new FailedExecutionServiceException(serviceException);

            var expectedExecutionServiceException =
                new ExecutionServiceException(failedExecutionServiceException);

            this.executionBrokerMock.Setup(broker =>
                broker.RunAsync(someExecutions, somePath))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> runTask = this.executionService.RunAsync(someExecutions, somePath);

            ExecutionServiceException actualExecutionServiceException =
                await Assert.ThrowsAsync<ExecutionServiceException>(runTask.AsTask);

            // then
            actualExecutionServiceException.Should().BeEquivalentTo(expectedExecutionServiceException);

            this.executionBrokerMock.Verify(broker =>
                broker.RunAsync(someExecutions, somePath),
                    Times.Once);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
