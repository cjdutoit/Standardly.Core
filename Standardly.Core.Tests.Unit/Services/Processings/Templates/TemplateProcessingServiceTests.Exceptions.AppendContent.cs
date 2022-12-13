// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Moq;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationOnAppendContentIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatchForAppendForAppend = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = true;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .Throws(dependencyValidationException);

            // when
            Action appendContentAction = () =>
                this.templateProcessingService
                    .AppendContent(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppendForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingDependencyValidationException actualException =
                Assert.Throws<TemplateProcessingDependencyValidationException>(appendContentAction);

            this.templateServiceMock.Verify(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist),
                        Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyOnAppendContentIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatchForAppendForAppend = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .Throws(dependencyException);

            // when
            Action appendContentAction = () =>
                this.templateProcessingService
                    .AppendContent(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppendForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingDependencyException actualException =
                Assert.Throws<TemplateProcessingDependencyException>(appendContentAction);

            this.templateServiceMock.Verify(service =>
                 service.AppendContent(
                     sourceContent,
                     doesNotContainContent,
                     regexToMatchForAppendForAppend,
                     appendContent,
                     appendToBeginning,
                     appendEvenIfContentAlreadyExist),
                         Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformation(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplate(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnAppendContentIfServiceErrorOccursAndLogIt()
        {
            // given
            string sourceContent = GetRandomString();
            string regexToMatchForAppend = GetRandomString();
            string appendContent = GetRandomString();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;

            var serviceException = new Exception();

            var failedTemplateProcessingServiceException =
                new FailedTemplateProcessingServiceException(serviceException);

            var expectedTemplateProcessingServiveException =
                new TemplateProcessingServiceException(
                    failedTemplateProcessingServiceException);

            this.templateServiceMock.Setup(service =>
                service.AppendContent(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .Throws(serviceException);

            // when
            Action appendContentAction = () =>
                this.templateProcessingService
                    .AppendContent(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingServiceException actualException =
                Assert.Throws<TemplateProcessingServiceException>(appendContentAction);

            this.templateServiceMock.Verify(service =>
                 service.AppendContent(
                     sourceContent,
                     doesNotContainContent,
                     regexToMatchForAppend,
                     appendContent,
                     appendToBeginning,
                     appendEvenIfContentAlreadyExist),
                         Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
