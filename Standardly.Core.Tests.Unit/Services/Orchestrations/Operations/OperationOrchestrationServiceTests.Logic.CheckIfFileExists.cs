// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldCheckIfFileExists()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool fileCheckResult = true;
            bool expectedResult = fileCheckResult;

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(randomPath))
                    .ReturnsAsync(fileCheckResult);

            // when
            bool actualResult = await this.operationOrchestrationService
                .CheckIfFileExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputFilePath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
