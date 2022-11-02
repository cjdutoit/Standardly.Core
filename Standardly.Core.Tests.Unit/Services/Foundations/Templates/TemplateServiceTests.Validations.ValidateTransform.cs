﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ShouldThrowValidationExceptionOnValidateTransformIfStringArgumentsInvalid(string invalidString)
        {
            // given
            string invalidContent = invalidString;
            char tagCharacter = '$';

            var invalidArgumentTemplateException =
               new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "content",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            System.Action validateTransformationAction = () =>
                this.templateService.ValidateTransformation(invalidContent, tagCharacter);

            TemplateValidationException actualTemplateValidationException =
                Assert.Throws<TemplateValidationException>(validateTransformationAction);

            // then
            actualTemplateValidationException.Should().BeEquivalentTo(expectedTemplateValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTemplateValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
