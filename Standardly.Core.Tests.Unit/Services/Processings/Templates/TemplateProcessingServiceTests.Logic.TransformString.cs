// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public void ShouldTransformString()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            string randomTransformedString = GetRandomString();
            string expectedTransformedString = randomTransformedString;
            Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
            Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;

            this.templateServiceMock.Setup(service =>
                service.TransformString(inputString, inputReplacementDictionary))
                    .Returns(expectedTransformedString);

            // when
            string actualTransformedString = this.templateProcessingService
                .TransformString(inputString, inputReplacementDictionary);

            // then
            actualTransformedString.Should().BeEquivalentTo(expectedTransformedString);

            this.templateServiceMock.Verify(service =>
                service.TransformString(inputString, inputReplacementDictionary),
                    Times.Once());

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformation(expectedTransformedString),
                    Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
