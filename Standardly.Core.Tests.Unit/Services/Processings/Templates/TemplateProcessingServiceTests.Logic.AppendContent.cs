// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public void ShouldAppendContent()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = true;
            bool appendEvenIfContentAlreadyExist = false;
            string output = GetRandomString();
            string expectedResult = output;

            this.templateServiceMock.Setup(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .Returns(output);

            // when
            string actualResult = this.templateProcessingService
                .AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.templateServiceMock.Verify(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist),
                        Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
