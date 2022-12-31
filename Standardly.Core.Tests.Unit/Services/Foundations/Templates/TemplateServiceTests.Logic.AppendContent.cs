// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Fact]
        public async Task ShouldAppendContent()
        {
            // given
            string sourceContent = GetRandomString();
            string appendContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            bool appendToBeginning = true;
            string doesNotContainContent = string.Empty;
            bool appendEvenIfContentAlreadyExist = false;
            bool matchFound = true;
            string matchedContent = GetRandomString();
            var expressionMatchResult = (matchFound, matchedContent);
            string mergedContent = appendContent + matchedContent;
            string expectedResult = GetRandomString();

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent))
                       .Returns(expressionMatchResult);

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, It.IsAny<string>()))
                       .Returns(expectedResult);

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent),
                       Times.Once);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, mergedContent),
                       Times.Once);

            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotAppendContent()
        {
            // given
            string sourceContent = GetRandomString();
            string appendContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            bool appendToBeginning = true;
            string doesNotContainContent = string.Empty;
            bool appendEvenIfContentAlreadyExist = false;
            bool matchFound = true;
            string matchedContent = appendContent;
            var expressionMatchResult = (matchFound, matchedContent);
            string mergedContent = appendContent + "\r\n" + matchedContent;
            string expectedResult = sourceContent;

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent))
                       .Returns(expressionMatchResult);

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, It.IsAny<string>()))
                       .Returns(expectedResult);

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent),
                       Times.Once);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, mergedContent),
                       Times.Never);

            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAppendIfTheDoesNotContainContentIsNotPresent()
        {
            // given
            string innerContent = GetRandomString();
            string sourceContent = GetRandomString() + innerContent + GetRandomString();
            string appendContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            bool appendToBeginning = true;
            string doesNotContainContent = GetRandomString();
            bool appendEvenIfContentAlreadyExist = false;
            bool matchFound = true;
            string matchedContent = GetRandomString();
            var expressionMatchResult = (matchFound, matchedContent);
            string mergedContent = appendContent + matchedContent;
            string expectedResult = GetRandomString();

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent))
                       .Returns(expressionMatchResult);

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, It.IsAny<string>()))
                       .Returns(expectedResult);

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent),
                       Times.Once);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, mergedContent),
                       Times.Once);

            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.fileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotAppendIfTheDoesNotContainContentIsPresent()
        {
            // given
            string innerContent = GetRandomString();
            string sourceContent = GetRandomString() + innerContent + GetRandomString();
            string appendContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            bool appendToBeginning = true;
            string doesNotContainContent = innerContent;
            bool appendEvenIfContentAlreadyExist = false;
            bool matchFound = true;
            string matchedContent = GetRandomString();
            var expressionMatchResult = (matchFound, matchedContent);
            string mergedContent = appendContent + "\r\n" + matchedContent;
            string expectedResult = sourceContent;

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent))
                       .Returns(expressionMatchResult);

            this.regularExpressionBrokerMock.Setup(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, It.IsAny<string>()))
                       .Returns(expectedResult);

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.CheckForExpressionMatch(regexToMatchForAppend, sourceContent),
                       Times.Never);

            this.regularExpressionBrokerMock.Verify(broker =>
               broker.Replace(sourceContent, regexToMatchForAppend, mergedContent),
                       Times.Never);

            this.regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.fileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
