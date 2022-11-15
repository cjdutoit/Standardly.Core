// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        [Fact]
        public void ShouldWriteToFileAsync()
        {
            // given
            string randomPath = GetRandomString();
            string inputFilePath = randomPath;
            string randomContent = GetRandomString();
            string inputContent = randomContent;

            this.fileServiceMock.Setup(service =>
                service.CheckIfDirectoryExists(It.IsAny<string>()))
                    .Returns(false);

            // when
            this.fileProcessingService.WriteToFile(inputFilePath, inputContent);

            // then
            this.fileServiceMock.Verify(service =>
                service.CheckIfDirectoryExists(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.CreateDirectory(It.IsAny<string>()),
                    Times.Once);

            this.fileServiceMock.Verify(service =>
                service.WriteToFile(inputFilePath, inputContent),
                    Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
