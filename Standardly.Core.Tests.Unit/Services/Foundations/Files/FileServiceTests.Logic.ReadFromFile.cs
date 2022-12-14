// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public void ShouldReadFromFile()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string outputResult = GetRandomString();
            string expectedResult = outputResult;

            this.fileBrokerMock.Setup(broker =>
                broker.ReadFile(inputFilePath))
                    .Returns(outputResult);

            // when
            string actualResult = this.fileService.ReadFromFile(inputFilePath);

            // then
            actualResult.Should().Be(expectedResult);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadFile(inputFilePath),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
