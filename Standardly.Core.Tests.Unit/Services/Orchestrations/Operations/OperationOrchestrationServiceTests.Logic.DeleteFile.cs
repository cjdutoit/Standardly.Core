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
        public async Task ShouldDeleteFileAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomContent = GetRandomString();
            string inputContent = randomContent;

            // when
            await this.operationOrchestrationService.DeleteFileAsync(inputFilePath);

            // then
            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteFileAsync(inputFilePath),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
