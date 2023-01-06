// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Fact]
        public async Task ShouldCheckIfFileExistsAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool fileCheckResult = true;
            bool expectedResult = fileCheckResult;

            this.fileServiceMock.Setup(service =>
                service.CheckIfFileExistsAsync(randomPath))
                    .ReturnsAsync(fileCheckResult);

            // when
            bool actualResult = await this.fileProcessingService
                .CheckIfFileExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileServiceMock.Verify(service =>
                service.CheckIfFileExistsAsync(inputFilePath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
