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
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRunExecutionAsync()
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;
            string randomExecutionResult = GetRandomString();
            string expectedResult = randomExecutionResult;

            this.executionProcessingServiceMock.Setup(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder))
                    .ReturnsAsync(randomExecutionResult);

            // when
            string actualResult = await this.operationOrchestrationService
                .RunAsync(inputExecutions, inputExecutionFolder);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.executionProcessingServiceMock.Verify(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder),
                    Times.Once());

            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
