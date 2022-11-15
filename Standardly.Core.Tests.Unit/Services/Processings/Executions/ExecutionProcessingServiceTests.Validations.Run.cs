// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public void ShouldThrowValidationExceptionOnRunIfExecutionsIsNullAndLogIt()
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
            Action runAction = () =>
                this.executionProcessingService.Run(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                Assert.Throws<ExecutionProcessingValidationException>(runAction);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionProcessingValidationException))),
                        Times.Once);

            this.executionServiceMock.Verify(service =>
                service.Run(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnRunIfExecutionFolderIsInvalidAndLogIt(string invalidValue)
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
            Action runAction = () =>
                this.executionProcessingService.Run(nullExecutions, executionFolder);

            ExecutionProcessingValidationException actualExecutionProcessingValidationException =
                Assert.Throws<ExecutionProcessingValidationException>(runAction);

            // then
            actualExecutionProcessingValidationException.Should()
                .BeEquivalentTo(expectedExecutionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExecutionProcessingValidationException))),
                        Times.Once);

            this.executionServiceMock.Verify(service =>
                service.Run(nullExecutions, executionFolder),
                    Times.Never);

            this.executionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
