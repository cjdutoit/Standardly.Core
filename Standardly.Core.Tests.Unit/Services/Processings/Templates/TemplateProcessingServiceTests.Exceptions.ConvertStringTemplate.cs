﻿// ---------------------------------------------------------------
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
        public void ShouldThrowDependencyValidationOnConvertStringTemplateIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string inputContent = randomString;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplate(inputContent))
                    .Throws(dependencyValidationException);

            // when
            Action convertStringToTemplateAction = () =>
                this.templateProcessingService.ConvertStringToTemplate(inputContent);

            // then
            TemplateProcessingDependencyValidationException actualException =
                Assert.Throws<TemplateProcessingDependencyValidationException>(
                    convertStringToTemplateAction);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplate(inputContent),
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
        public void ShouldThrowDependencyOnConvertStringTemplateIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            string randomString = GetRandomString();
            string inputContent = randomString;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplate(inputContent))
                    .Throws(dependencyException);

            // when
            Action convertStringToTemplateAction = () =>
                this.templateProcessingService.ConvertStringToTemplate(inputContent);

            // then
            TemplateProcessingDependencyException actualException =
                Assert.Throws<TemplateProcessingDependencyException>(convertStringToTemplateAction);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplate(inputContent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingDependencyException))),
                        Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnConvertStringTemplateIfServiceErrorOccursAndLogIt()
        {
            // given
            string randomString = GetRandomString();
            string inputContent = randomString;

            var serviceException = new Exception();

            var failedTemplateProcessingServiceException =
                new FailedTemplateProcessingServiceException(serviceException);

            var expectedTemplateProcessingServiveException =
                new TemplateProcessingServiceException(
                    failedTemplateProcessingServiceException);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplate(inputContent))
                    .Throws(serviceException);

            // when
            Action convertStringToTemplateAction = () =>
                this.templateProcessingService.ConvertStringToTemplate(inputContent);

            // then
            TemplateProcessingServiceException actualException =
                Assert.Throws<TemplateProcessingServiceException>(convertStringToTemplateAction);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplate(inputContent),
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
