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
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnTransformStringIfDependencyValidationErrorOccursAsync(
            Xeption dependencyValidationException)
        {
            // given
            string randomInputString = GetRandomString();
            string inputString = randomInputString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var expectedTemplateProcessingDependencyValidationException =
                new TemplateProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> transformTemplateTask =
                this.templateProcessingService
                    .TransformStringAsync(inputString, inputReplacementDictionary);

            TemplateProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyValidationException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyValidationException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary),
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
        public async Task ShouldThrowDependencyOnTransformStringIfDependencyErrorOccursAsync(
            Xeption dependencyException)
        {
            // given
            string randomInputString = GetRandomString();
            string inputString = randomInputString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var expectedTemplateProcessingDependencyException =
                new TemplateProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> transformTemplateTask =
                this.templateProcessingService
                    .TransformStringAsync(inputString, inputReplacementDictionary);

            TemplateProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TemplateProcessingDependencyException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingDependencyException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary),
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
        public async Task ShouldThrowServiceExceptionOnTransformStringIfServiceErrorOccursAsync()
        {
            // given
            string randomInputString = GetRandomString();
            string inputString = randomInputString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            var serviceException = new Exception();

            var failedTemplateProcessingServiceException =
                new FailedTemplateProcessingServiceException(serviceException);

            var expectedTemplateProcessingServiveException =
                new TemplateProcessingServiceException(
                    failedTemplateProcessingServiceException);

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> transformTemplateTask =
                this.templateProcessingService
                    .TransformStringAsync(inputString, inputReplacementDictionary);

            TemplateProcessingServiceException actualException =
                await Assert.ThrowsAsync<TemplateProcessingServiceException>(transformTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingServiveException);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary),
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
