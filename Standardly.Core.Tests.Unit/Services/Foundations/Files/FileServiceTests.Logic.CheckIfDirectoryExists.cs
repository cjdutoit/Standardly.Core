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
        public async Task ShouldCheckIfDirectoryExists()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            bool outputResult = true;
            bool expectedResult = outputResult;

            this.fileBrokerMock.Setup(broker =>
                broker.CheckIfDirectoryExistsAsync(inputFilePath))
                    .ReturnsAsync(outputResult);

            // when
            bool actualResult = await this.fileService
                .CheckIfDirectoryExistsAsync(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileBrokerMock.Verify(broker =>
                broker.CheckIfDirectoryExistsAsync(inputFilePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
