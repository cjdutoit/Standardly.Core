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
        public async Task ShouldDeleteDirectoryAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool recursive = true;

            // when
            await this.operationOrchestrationService
                .DeleteDirectoryAsync(inputFilePath, recursive);

            // then
            this.fileProcessingServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputFilePath, recursive),
                    Times.Once);

            this.fileProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
