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
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Executions
{
    public partial class ExecutionServiceTests
    {
        [Fact]
        public async Task ShouldRunExecutionsAsync()
        {
            // given
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string randomOutput = GetRandomString();
            string expectedResult = randomOutput;

            this.executionBrokerMock.Setup(broker =>
                broker.RunAsync(inputExecutions, inputFilePath))
                    .ReturnsAsync(randomOutput);

            // when
            string actualResult = await this.executionService.RunAsync(inputExecutions, inputFilePath);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.executionBrokerMock.Verify(broker =>
                broker.RunAsync(inputExecutions, inputFilePath),
                    Times.Once);

            this.executionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
