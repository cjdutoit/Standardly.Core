// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public async Task ShouldWriteToFile()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            string randomContent = GetRandomString();
            string inputContent = randomContent;

            // when
            await this.fileService.WriteToFileAsync(inputFilePath, inputContent);

            // then
            this.fileBrokerMock.Verify(broker =>
                broker.WriteToFileAsync(inputFilePath, inputContent),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}