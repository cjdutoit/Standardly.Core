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
using Standardly.Core.Models.Services.Processings.Executions.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Executions
{
    public partial class ExecutionProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRunIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;

            var expectedExecutionProcessingDependencyValidationException =
                new ExecutionProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.executionServiceMock.Setup(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> runTask =
                this.executionProcessingService.RunAsync(randomExecutions, inputExecutionFolder);

            ExecutionProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ExecutionProcessingDependencyValidationException>(runTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedExecutionProcessingDependencyValidationException);

            this.executionServiceMock.Verify(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder),
                    Times.Once);

            this.executionServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRunIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;

            var expectedExecutionProcessingDependencyException =
                new ExecutionProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.executionServiceMock.Setup(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> runTask =
                this.executionProcessingService.RunAsync(randomExecutions, inputExecutionFolder);

            ExecutionProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ExecutionProcessingDependencyException>(runTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedExecutionProcessingDependencyException);

            this.executionServiceMock.Verify(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder),
                    Times.Once);

            this.executionServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRunIfServiceErrorOccursAsync()
        {
            // given
            string randomExecutionFolder = GetRandomString();
            string inputExecutionFolder = randomExecutionFolder;
            List<Execution> randomExecutions = GetRandomExecutions();
            List<Execution> inputExecutions = randomExecutions;

            var serviceException = new Exception();

            var failedExecutionProcessingServiceException =
                new FailedExecutionProcessingServiceException(serviceException);

            var expectedExecutionProcessingServiveException =
                new ExecutionProcessingServiceException(
                    failedExecutionProcessingServiceException);

            this.executionServiceMock.Setup(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> runTask =
                this.executionProcessingService.RunAsync(randomExecutions, inputExecutionFolder);

            ExecutionProcessingServiceException actualException =
                await Assert.ThrowsAsync<ExecutionProcessingServiceException>(runTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedExecutionProcessingServiveException);

            this.executionServiceMock.Verify(service =>
                service.RunAsync(inputExecutions, inputExecutionFolder),
                    Times.Once);

            this.executionServiceMock.VerifyNoOtherCalls();
        }
    }
}
