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
        public async Task ShouldThrowDependencyValidationOnTransformTemplateIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
            char inputTagCharacter = '$';

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary, inputTagCharacter);

            // then
            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingDependencyValidationException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>(), inputTagCharacter),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnTransformTemplateIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
            char inputTagCharacter = '$';

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<Template> transformTemplateTask =
                this.templateProcessingService
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary, inputTagCharacter);

            // then
            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingDependencyException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>(), inputTagCharacter),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnTransformTemplateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
            char inputTagCharacter = '$';

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
                    .TransformTemplateAsync(inputTemplate, inputReplacementDictionary, inputTagCharacter);

            // then
            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(transformTemplateTask.AsTask);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateProcessingServiveException))),
                        Times.Once);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(It.IsAny<string>(), inputTagCharacter),
                    Times.Never());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
