// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnTransformTemplateIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            randomInputTemplate.ReplacementDictionary = randomReplacementDictionary;
            Template inputTemplate = randomInputTemplate;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputTemplate.ReplacementDictionary))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate);

            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyValidationException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputTemplate.ReplacementDictionary),
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
        public async Task ShouldThrowDependencyOnTransformTemplateIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            randomInputTemplate.ReplacementDictionary = randomReplacementDictionary;
            Template inputTemplate = randomInputTemplate;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputTemplate.ReplacementDictionary))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate);

            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputTemplate.ReplacementDictionary),
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
        public async Task ShouldThrowServiceExceptionOnTransformTemplateIfServiceErrorOccursAsync()
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            randomInputTemplate.ReplacementDictionary = randomReplacementDictionary;
            Template inputTemplate = randomInputTemplate;

            var serviceException = new Exception();

            var failedTemplateProcessingServiceException =
                new FailedTemplateProcessingServiceException(serviceException);

            var expectedTemplateProcessingServiveException =
                new TemplateProcessingServiceException(
                    failedTemplateProcessingServiceException);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, randomInputTemplate.ReplacementDictionary))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate);

            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingServiveException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, randomInputTemplate.ReplacementDictionary),
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
