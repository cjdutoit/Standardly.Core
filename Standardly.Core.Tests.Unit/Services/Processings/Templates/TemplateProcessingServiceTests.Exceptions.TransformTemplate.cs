// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnTransformTemplateIfDependencyValidationErrorOccursAndLogIt(
            Xeption dependencyValidationException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary);

            // then
            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnTransformTemplateIfDependencyErrorOccursAndLogIt(
            Xeption dependencyException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary);

            // then
            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
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
        public async Task ShouldThrowServiceExceptionOnTransformTemplateIfServiceErrorOccursAndLogIt()
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var serviceException = new Exception();

            var failedTemplateProcessingServiceException =
                new FailedTemplateProcessingServiceException(serviceException);

            var expectedTemplateProcessingServiveException =
                new TemplateProcessingServiceException(
                    failedTemplateProcessingServiceException);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary);

            // then
            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once());

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
