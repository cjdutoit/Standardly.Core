// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Executions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Executions
{
    public partial class ExecutionServiceTests
    {
        [Fact]
        public void ShouldRunExecutions()
        {
            // given
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string randomOutput = GetRandomString();
            string expectedResult = randomOutput;

            this.executionBrokerMock.Setup(broker =>
                broker.Run(inputExecutions, inputFilePath))
                    .Returns(randomOutput);

            // when
            string actualResult = this.executionService.Run(inputExecutions, inputFilePath);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.executionBrokerMock.Verify(broker =>
                broker.Run(inputExecutions, inputFilePath),
                    Times.Once);

            this.executionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
