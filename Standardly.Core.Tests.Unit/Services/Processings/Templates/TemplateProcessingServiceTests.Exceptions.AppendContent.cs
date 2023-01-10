// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAppendContentIfDependencyValidationErrorOccursAsync(
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

            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(appendContentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyValidationException);

            this.templateServiceMock.Verify(service =>
                service.AppendContentAsync(
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
        public async Task ShouldThrowDependencyOnAppendContentIfDependencyErrorOccursAsync(
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

            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(appendContentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyException);

            this.templateServiceMock.Verify(service =>
                 service.AppendContentAsync(
                     sourceContent,
                     doesNotContainContent,
                     regexToMatchForAppendForAppend,
                     appendContent,
                     appendToBeginning,
                     appendEvenIfContentAlreadyExist),
                         Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAppendContentIfServiceErrorOccursAsync()
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

            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(appendContentTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingServiveException);

            this.templateServiceMock.Verify(service =>
                 service.AppendContentAsync(
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
