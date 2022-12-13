// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Processings.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnTransformStringIfArgumentsIsInvalidAndLogIt(
            string invalidInput)
        {
            // given
            string invalidContent = invalidInput;
            Dictionary<string, string> nullDictionary = null;

            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            invalidArgumentTemplateProcessingException.AddData(
                key: "content",
                values: "Text is required");

            invalidArgumentTemplateProcessingException.AddData(
                key: "replacementDictionary",
                values: "Dictionary values is required");

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            Action transformStringAction = () =>
                this.templateProcessingService
                    .TransformString(invalidContent, nullDictionary);

            TemplateProcessingValidationException actualException =
                Assert.Throws<TemplateProcessingValidationException>(transformStringAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.TransformString(It.IsAny<string>(), nullDictionary),
                    Times.Never);

            this.templateServiceMock.Verify(service =>
                service.ValidateTransformation(It.IsAny<string>()),
                    Times.Never());

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
