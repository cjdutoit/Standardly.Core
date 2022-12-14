// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowValidationExceptionIfTemplateContentIsInvalidAndLogIt(
            string invalidTemplateContent)
        {
            // given
            var invalidArgumentTemplateProcessingException =
                new InvalidArgumentTemplateProcessingException();

            invalidArgumentTemplateProcessingException.AddData(
                key: "content",
                values: "Text is required");

            var expectedTemplateProcessingValidationException =
                new TemplateProcessingValidationException(invalidArgumentTemplateProcessingException);

            // when
            Action convertStringToTemplateAction = () =>
                this.templateProcessingService.ConvertStringToTemplate(invalidTemplateContent);

            TemplateProcessingValidationException actualException =
                Assert.Throws<TemplateProcessingValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplate(invalidTemplateContent),
                    Times.Never);

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
