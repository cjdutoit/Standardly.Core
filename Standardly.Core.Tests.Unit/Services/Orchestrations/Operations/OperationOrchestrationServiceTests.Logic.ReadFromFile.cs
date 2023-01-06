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
        public async Task ShouldReadFromFileAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomContent = GetRandomString();
            string expectedResult = randomContent;

            this.fileProcessingServiceMock.Setup(service =>
                service.ReadFromFileAsync(randomPath))
                    .ReturnsAsync(expectedResult);

            // when
            string actualResult =
                await this.operationOrchestrationService.ReadFromFileAsync(inputFilePath);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.fileProcessingServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputFilePath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
