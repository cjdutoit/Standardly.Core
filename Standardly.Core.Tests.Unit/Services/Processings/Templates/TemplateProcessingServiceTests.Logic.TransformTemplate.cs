// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public async Task ShouldTransformTemplateAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            string randomTransformedString = GetRandomString();
            string transformedString = randomTransformedString;
            char inputTagCharacter = '$';
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
            Template randomInputTemplate = CreateRandomTemplate();
            Template inputTemplate = randomInputTemplate;
            Template randomTemplate = CreateRandomTemplate();
            Template expectedTemplate = randomTemplate;

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary))
                    .ReturnsAsync(transformedString);

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplateAsync(transformedString))
                    .ReturnsAsync(expectedTemplate);

            // when
            Template actualTemplate = await this.templateProcessingService
                .TransformTemplateAsync(inputTemplate, inputReplacementDictionary, inputTagCharacter);

            // then
            actualTemplate.Should().BeEquivalentTo(expectedTemplate);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputTemplate.RawTemplate, inputReplacementDictionary),
                    Times.Once());

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(transformedString, inputTagCharacter),
                    Times.Once());

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(transformedString),
                    Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
