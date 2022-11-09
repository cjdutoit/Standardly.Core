// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyValidationOnAppendContentIfDependencyValidationErrorOccursAndLogItAsync(
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
                service.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> appendContentTask =
                this.templateProcessingService
                    .AppendContentAsync(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppendForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(appendContentTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingDependencyValidationException))),
                        Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnAppendContentIfDependencyErrorOccursAndLogItAsync(
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
                service.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppendForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> appendContentTask =
                this.templateProcessingService
                    .AppendContentAsync(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppendForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(appendContentTask.AsTask);

            this.templateServiceMock.Verify(service =>
                 service.AppendContentAsync(
                     sourceContent,
                     doesNotContainContent,
                     regexToMatchForAppendForAppend,
                     appendContent,
                     appendToBeginning,
                     appendEvenIfContentAlreadyExist),
                         Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingDependencyException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAppendContentIfServiceErrorOccursAndLogItAsync()
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
                service.AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatchForAppend,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<string> appendContentTask =
                this.templateProcessingService
                    .AppendContentAsync(
                        sourceContent,
                        doesNotContainContent,
                        regexToMatchForAppend,
                        appendContent,
                        appendToBeginning,
                        appendEvenIfContentAlreadyExist);

            // then
            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(appendContentTask.AsTask);

            this.templateServiceMock.Verify(service =>
                 service.AppendContentAsync(
                     sourceContent,
                     doesNotContainContent,
                     regexToMatchForAppend,
                     appendContent,
                     appendToBeginning,
                     appendEvenIfContentAlreadyExist),
                         Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingServiveException))),
                        Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
