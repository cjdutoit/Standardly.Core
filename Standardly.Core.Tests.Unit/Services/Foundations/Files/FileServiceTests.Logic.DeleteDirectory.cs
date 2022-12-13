// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        [Fact]
        public void ShouldDeleteDirectory()
        {
            // given
            string randomFilePath = GetRandomString();
            string inputFilePath = randomFilePath;
            bool recursive = true;

            // when
            this.fileService.DeleteDirectory(inputFilePath, recursive);

            // then
            this.fileBrokerMock.Verify(broker =>
                broker.DeleteDirectory(inputFilePath, recursive),
                    Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
