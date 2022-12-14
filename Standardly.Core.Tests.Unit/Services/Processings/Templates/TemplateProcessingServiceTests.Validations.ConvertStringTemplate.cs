// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Processings.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionIfTemplateContentIsInvalidAsync(
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
            ValueTask<Template> convertStringToTemplateTask =
                this.templateProcessingService.ConvertStringToTemplateAsync(invalidTemplateContent);

            TemplateProcessingValidationException actualException =
                await Assert.ThrowsAsync<TemplateProcessingValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateProcessingValidationException);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(invalidTemplateContent),
                    Times.Never);

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
