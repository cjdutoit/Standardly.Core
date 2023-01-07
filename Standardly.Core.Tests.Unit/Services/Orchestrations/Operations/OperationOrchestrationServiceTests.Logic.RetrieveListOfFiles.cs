// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfFilesAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomSearchPattern = GetRandomString();
            string inputSearchPattern = randomSearchPattern;
            List<string> randomOutput = GetRandomStringList();
            List<string> expectedResult = randomOutput;

            this.fileProcessingServiceMock.Setup(service =>
                service.RetrieveListOfFilesAsync(inputFilePath, inputSearchPattern))
                    .ReturnsAsync(expectedResult);

            // when
            List<string> actualResult =
                await this.operationOrchestrationService
                    .RetrieveListOfFilesAsync(inputFilePath, inputSearchPattern);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.fileProcessingServiceMock.Verify(service =>
                service.RetrieveListOfFilesAsync(inputFilePath, inputSearchPattern),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
