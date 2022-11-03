﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Models.Foundations.Executions;
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
        public async Task ShouldThrowValidationExceptionOnConvertStringToTemplateIfContentIsNullOrEmptyAsync(
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
        public async Task ShouldThrowValidationExceptionOnConvertStringToTemplateIfTemplateIsInvalidAsync(
            string invalidString)
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertIfTemplateTasksIsInvalidAsync(string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = GetRandomString(),
                Description = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Models.Foundations.Templates.Tasks.Task someTask = new Models.Foundations.Templates.Tasks.Task()
            {
                Name = invalidString,
            };

            someTemplate.Tasks.Add(someTask);
            string someStringTemplate = SerializeTemplate(someTemplate);
            string inputStringTemplate = someStringTemplate;

            var invalidTemplateException =
                new InvalidTemplateException();

            invalidTemplateException.AddData(
                key: "Tasks[0].Name",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Tasks[0].Actions",
                values: "Actions is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(inputStringTemplate);

            TemplateValidationException actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertIfTemplateTaskActionsIsInvalidAsync(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = GetRandomString(),
                Description = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Models.Foundations.Templates.Tasks.Task someTask = new Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                Actions = new List<Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Models.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = invalidString,
                        Files = new List<Models.Foundations.Templates.Tasks.Actions.Files.File>(),
                        Executions = new List<Execution>(),
                    }
                }
            };

            someTemplate.Tasks.Add(someTask);
            string someStringTemplate = SerializeTemplate(someTemplate);
            string inputStringTemplate = someStringTemplate;

            var invalidTemplateException =
                new InvalidTemplateException();

            invalidTemplateException.AddData(
                key: "Actions[0].Name",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Actions[0].Executions",
                values: "Executions is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(inputStringTemplate);

            TemplateValidationException actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionWhenTemplateTaskActionFileItemIsInvalid(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = GetRandomString(),
                Description = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Models.Foundations.Templates.Tasks.Task someTask = new Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                Actions = new List<Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Models.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<Models.Foundations.Templates.Tasks.Actions.Files.File>()
                        {
                            new Models.Foundations.Templates.Tasks.Actions.Files.File()
                            {
                                Template = invalidString,
                                Target = invalidString
                            },
                        },
                        Executions = new List<Execution>()
                        {
                            new Execution()
                            {
                               Name = GetRandomString(),
                               Instruction = GetRandomString(),
                            },
                        },
                    }
                }
            };

            someTemplate.Tasks.Add(someTask);
            string someStringTemplate = SerializeTemplate(someTemplate);
            string inputStringTemplate = someStringTemplate;

            var invalidTemplateException =
                new InvalidTemplateException();

            invalidTemplateException.AddData(
                key: "Actions[0].Files[0].Template",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Actions[0].Files[0].Target",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(inputStringTemplate);

            TemplateValidationException actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnConvertIfTemplateTaskActionExecutionsIsInvalid(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = GetRandomString(),
                Description = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Models.Foundations.Templates.Tasks.Task someTask = new Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                Actions = new List<Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Models.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<Models.Foundations.Templates.Tasks.Actions.Files.File>()
                        {
                            new Models.Foundations.Templates.Tasks.Actions.Files.File()
                            {
                                Template = GetRandomString(),
                                Target = GetRandomString()
                            },
                        },
                        Executions = new List<Execution>()
                        {
                            new Execution()
                            {
                               Name = invalidString,
                               Instruction = invalidString,
                            },
                        },
                    }
                }
            };

            someTemplate.Tasks.Add(someTask);
            string someStringTemplate = SerializeTemplate(someTemplate);
            string inputStringTemplate = someStringTemplate;

            var invalidTemplateException =
                new InvalidTemplateException();

            invalidTemplateException.AddData(
                key: "Actions[0].Executions[0].Name",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Actions[0].Executions[0].Instruction",
                values: "Text is required");

            var expectedTemplateValidationException =
                new TemplateValidationException(invalidTemplateException);

            // when
            ValueTask<Template> convertStringToTemplateTask =
                this.templateService.ConvertStringToTemplateAsync(inputStringTemplate);

            TemplateValidationException actualException =
                await Assert.ThrowsAsync<TemplateValidationException>(convertStringToTemplateTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
