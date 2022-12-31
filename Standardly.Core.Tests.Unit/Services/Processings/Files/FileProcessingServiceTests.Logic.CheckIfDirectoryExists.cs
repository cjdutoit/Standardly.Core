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
        public async Task ShouldCheckIfDirectoryExists()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            bool expectedResult = true;

            this.fileServiceMock.Setup(service =>
                service.CheckIfDirectoryExistsAsync(randomPath))
                    .ReturnsAsync(expectedResult);

            // when
            bool actualResult = await this.fileProcessingService
                .CheckIfDirectoryExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExistsAsync(inputFilePath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
