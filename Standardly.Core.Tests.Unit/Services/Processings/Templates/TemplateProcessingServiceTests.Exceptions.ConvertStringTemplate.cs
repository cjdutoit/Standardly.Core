// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnConvertStringTemplateIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string inputContent = randomString;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplateAsync(inputContent))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateProcessingService.ConvertStringToTemplateAsync(inputContent);

            // then
            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(
                    convertStringToTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(inputContent),
                    Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnConvertStringTemplateIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomString = GetRandomString();
            string inputContent = randomString;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplateAsync(inputContent))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateProcessingService.ConvertStringToTemplateAsync(inputContent);

            // then
            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(convertStringToTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(inputContent),
                    Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnConvertStringTemplateIfServiceErrorOccursAsync()
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
                service.ConvertStringToTemplateAsync(inputContent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateProcessingService.ConvertStringToTemplateAsync(inputContent);

            // then
            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(convertStringToTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(inputContent),
                    Times.Once);

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
