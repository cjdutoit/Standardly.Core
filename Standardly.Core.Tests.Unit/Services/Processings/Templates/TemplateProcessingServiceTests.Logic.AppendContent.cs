// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAppendContentAsync()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatch = GetRandomString();
            string appendContent = GetRandomString();
            bool appendToBeginning = true;
            bool onlyAppendIfNotPresent = true;
            string output = GetRandomString();
            string expectedResult = output;

            this.templateServiceMock.Setup(service =>
                service.AppendContentAsync(
                    sourceContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    onlyAppendIfNotPresent))
                        .ReturnsAsync(output);

            // when
            string actualResult = await this.templateProcessingService
                .AppendContentAsync(
                    sourceContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    onlyAppendIfNotPresent);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.templateServiceMock.Verify(service =>
                service.AppendContentAsync(
                    sourceContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    onlyAppendIfNotPresent),
                        Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
