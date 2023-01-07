// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldWriteToFileAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomContent = GetRandomString();
            string inputContent = randomContent;

            this.fileProcessingServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(false);

            // when
            await this.operationOrchestrationService.WriteToFileAsync(inputFilePath, inputContent);

            // then
            this.fileProcessingServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(service =>
                service.CreateDirectoryAsync(It.IsAny<string>()),
                    Times.Once);

            this.fileProcessingServiceMock.Verify(service =>
                service.WriteToFileAsync(inputFilePath, inputContent),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
