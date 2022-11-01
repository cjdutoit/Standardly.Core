// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Fact]
        public async Task ShouldDeleteDirectoryAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool recursive = true;

            // when
            await this.fileProcessingService
                .DeleteDirectoryAsync(inputFilePath, recursive);

            // then
            this.fileServiceMock.Verify(service =>
                service.DeleteDirectoryAsync(inputFilePath, recursive),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
