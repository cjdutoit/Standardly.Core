// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertStringToTemplateIfContentIsNullOrEmpty(
            string invalidString)
        {
            // given
            string content = invalidString;

            var invalidArgumentTemplateException =
                            new InvalidArgumentTemplateException();

            invalidArgumentTemplateException.AddData(
                key: "content",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidArgumentTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(content);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertStringToTemplateIfTemplateIsInvalid(string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = invalidString,
                Description = invalidString,
                TemplateType = invalidString,
                ProjectsRequired = invalidString,
            };

            string someStringTemplate = SerializeTemplate(someTemplate);
            string inputStringTemplate = someStringTemplate;

            var invalidTemplateException =
                new InvalidTemplateException();

            invalidTemplateException.AddData(
                key: "Template Name",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Template Description",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Template Type",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Template Projects Required",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Template Tasks",
                values: "Tasks is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(inputStringTemplate);

            var actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
