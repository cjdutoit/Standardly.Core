// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Orchestrations.Operations.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveListOfFilesIfInputsIsInvalidAsync(
            string invalidInput)
        {
            // given
            string invalidPath = invalidInput;
            string invalidSearchPattern = invalidInput;

            var invalidArgumentOperationOrchestrationException =
                new InvalidArgumentOperationOrchestrationException();

            invalidArgumentOperationOrchestrationException.AddData(
                key: "path",
                values: "Text is required");

            invalidArgumentOperationOrchestrationException.AddData(
                key: "searchPattern",
                values: "Text is required");

            var expectedOperationOrchestrationValidationException =
                new OperationOrchestrationValidationException(invalidArgumentOperationOrchestrationException);

            // when
            ValueTask<List<string>> retrieveListOfFilesTask =
                this.operationOrchestrationService
                    .RetrieveListOfFilesAsync(path: invalidPath, searchPattern: invalidSearchPattern);

            OperationOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OperationOrchestrationValidationException>(retrieveListOfFilesTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOperationOrchestrationValidationException);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(invalidPath, invalidSearchPattern),
                    Times.Never);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
