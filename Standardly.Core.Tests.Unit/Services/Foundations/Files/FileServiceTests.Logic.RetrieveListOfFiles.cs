// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfFilesAsync()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string randomSearchPattern = GetRandomString();
            string inputSearchPattern = randomSearchPattern;
            List<string> randomResult = GetRandomStringList();
            List<string> outputResult = randomResult;
            List<string> expectedResult = randomResult;

            this.fileBrokerMock.Setup(broker =>
                broker.GetListOfFiles(inputFilePath, inputSearchPattern))
                    .Returns(outputResult);

            // when
            List<string> actualResult =
                await this.fileService.RetrieveListOfFilesAsync(inputFilePath, inputSearchPattern);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.fileBrokerMock.Verify(broker =>
                broker.GetListOfFiles(inputFilePath, inputSearchPattern),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
