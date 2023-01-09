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
        public async Task ShouldReadFromFileAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomContent = GetRandomString();
            string expectedResult = randomContent;

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(randomPath))
                    .ReturnsAsync(expectedResult);

            // when
            string actualResult =
                await this.fileProcessingService.ReadFromFileAsync(inputFilePath);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.fileServiceMock.Verify(service =>
                service.ReadFromFileAsync(inputFilePath),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}
