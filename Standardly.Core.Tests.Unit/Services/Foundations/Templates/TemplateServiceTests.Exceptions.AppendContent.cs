// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Standardly.Core.Services.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [MemberData(nameof(AppendContentDependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnAppendContentIfErrorOccursAndLogIt(
            Exception dependencyValidationException)
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatch = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            Mock<IRegularExpressionBroker> regularExpressionBrokerMock =
                new Mock<IRegularExpressionBroker>();

            TemplateService templateService = new TemplateService(
                fileBroker: fileBrokerMock.Object,
                regularExpressionBroker: regularExpressionBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);

            var invalidRegularExpressionTemplateException =
                new InvalidRegularExpressionTemplateException(
                    dependencyValidationException);

            var expectedTemplateDependencyValidationException =
                new TemplateDependencyValidationException(invalidRegularExpressionTemplateException);

            regularExpressionBrokerMock.Setup(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent))
                    .Throws(dependencyValidationException);

            // when
            Action templateServiceExceptionTask = () =>
                templateService.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            TemplateDependencyValidationException actualTemplateDependencyValidationException =
                Assert.Throws<TemplateDependencyValidationException>(templateServiceExceptionTask);

            // then
            actualTemplateDependencyValidationException.Should()
                .BeEquivalentTo(expectedTemplateDependencyValidationException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateDependencyValidationException))),
                        Times.Once);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnAppendContentIfServiceErrorOccurs()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatch = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            Mock<IRegularExpressionBroker> regularExpressionBrokerMock =
                new Mock<IRegularExpressionBroker>();

            TemplateService templateService = new TemplateService(
                fileBroker: fileBrokerMock.Object,
                regularExpressionBroker: regularExpressionBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);

            var serviceException = new Exception();

            var failedTemplateServiceException =
                new FailedTemplateServiceException(serviceException);

            var expectedTemplateServiceException =
                new TemplateServiceException(failedTemplateServiceException);

            regularExpressionBrokerMock.Setup(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent))
                    .Throws(serviceException);

            // when
            Action templateServiceExceptionTask = () =>
                templateService.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            TemplateServiceException actualTemplateServiceException =
                Assert.Throws<TemplateServiceException>(templateServiceExceptionTask);

            // then
            actualTemplateServiceException.Should().BeEquivalentTo(expectedTemplateServiceException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateServiceException))),
                        Times.Once);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            regularExpressionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}