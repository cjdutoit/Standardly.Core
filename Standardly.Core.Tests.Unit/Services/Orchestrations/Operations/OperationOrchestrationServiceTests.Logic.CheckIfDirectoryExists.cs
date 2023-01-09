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
        public async Task ShouldCheckIfDirectoryExistsAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool expectedResult = true;

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(randomPath))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.operationOrchestrationService
                .CheckIfDirectoryExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(inputFilePath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
