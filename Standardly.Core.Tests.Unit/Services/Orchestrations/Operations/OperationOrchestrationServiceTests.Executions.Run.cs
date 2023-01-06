// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Orchestrations.Operations.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRunIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;

            var expectedOperationOrchestrationDependencyValidationException =
                new OperationOrchestrationDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.executionProcessingServiceMock.Setup(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> runTask =
                this.operationOrchestrationService.RunAsync(randomExecutions, inputExecutionFolder);

            // then
            OperationOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationDependencyValidationException>(runTask.AsTask);

            this.executionProcessingServiceMock.Verify(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder),
                    Times.Once);

            this.executionProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
