// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public async Task ShouldReadFromFileAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string outputResult = GetRandomString();
            string expectedResult = outputResult;

            this.fileBrokerMock.Setup(broker =>
                broker.ReadFileAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            // when
            string actualResult = await this.fileService.ReadFromFileAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadFileAsync(inputFilePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
