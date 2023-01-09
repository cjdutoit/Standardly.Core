// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationExceptionOnAppendContentIfErrorOccursAsync(
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
                regularExpressionBroker: regularExpressionBrokerMock.Object);

            var invalidRegularExpressionTemplateException =
                new InvalidRegularExpressionTemplateException(
                    dependencyValidationException);

            var expectedTemplateDependencyValidationException =
                new TemplateDependencyValidationException(invalidRegularExpressionTemplateException);

            regularExpressionBrokerMock.Setup(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<string> templateServiceExceptionTask =
                templateService.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            TemplateDependencyValidationException actualTemplateDependencyValidationException =
                await Assert.ThrowsAsync<TemplateDependencyValidationException>(templateServiceExceptionTask.AsTask);

            // then
            actualTemplateDependencyValidationException.Should()
                .BeEquivalentTo(expectedTemplateDependencyValidationException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Once);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            regularExpressionBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudThrowServiceExceptionOnAppendContentIfServiceErrorOccursAsync()
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
                regularExpressionBroker: regularExpressionBrokerMock.Object);

            var serviceException = new Exception();

            var failedTemplateServiceException =
                new FailedTemplateServiceException(serviceException);

            var expectedTemplateServiceException =
                new TemplateServiceException(failedTemplateServiceException);

            regularExpressionBrokerMock.Setup(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent))
                    .Throws(serviceException);

            // when
            ValueTask<string> templateServiceExceptionTask =
                templateService.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            TemplateServiceException actualTemplateServiceException =
                await Assert.ThrowsAsync<TemplateServiceException>(templateServiceExceptionTask.AsTask);

            // then
            actualTemplateServiceException.Should().BeEquivalentTo(expectedTemplateServiceException);

            regularExpressionBrokerMock.Verify(broker =>
                broker.CheckForExpressionMatch(regexToMatch, sourceContent),
                    Times.Once);

            regularExpressionBrokerMock.Verify(broker =>
                broker.Replace(sourceContent, regexToMatch, appendContent),
                    Times.Never);

            this.fileBrokerMock.VerifyNoOtherCalls();
            regularExpressionBrokerMock.VerifyNoOtherCalls();
        }
    }
}