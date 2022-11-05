// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public async Task ShouldTransformStringAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            string randomTransformedString = GetRandomString();
            string expectedTransformedString = randomTransformedString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            this.templateServiceMock.Setup(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary))
                    .ReturnsAsync(expectedTransformedString);

            // when
            string actualTransformedString = await this.templateProcessingService
                .TransformStringAsync(inputString, inputReplacementDictionary);

            // then
            actualTransformedString.Should().BeEquivalentTo(expectedTransformedString);

            this.templateServiceMock.Verify(service =>
                service.TransformStringAsync(inputString, inputReplacementDictionary),
                    Times.Once());

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformationAsync(expectedTransformedString),
                    Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
