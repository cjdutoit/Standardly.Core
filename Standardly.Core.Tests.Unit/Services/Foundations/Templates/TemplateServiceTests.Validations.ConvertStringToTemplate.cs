// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Models.Services.Foundations.Executions;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Files;
using Xunit;
using Task = System.Threading.Tasks.Task;

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
                ModelSingularName = invalidString,
                ModelPluralName = invalidString,
                Name = invalidString,
                Description = invalidString,
                Stack = invalidString,
                Language = invalidString,
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
                key: "Template Stack",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Template Language",
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
                ModelSingularName = GetRandomString(),
                ModelPluralName = GetRandomString(),
                Name = GetRandomString(),
                Description = GetRandomString(),
                Stack = GetRandomString(),
                Language = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Services.Foundations.Templates.Tasks.Task someTask =
                new Core.Models.Services.Foundations.Templates.Tasks.Task()
                {
                    Name = invalidString,
                    BranchName = invalidString,
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
                key: "Tasks[0].BranchName",
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
                ModelSingularName = GetRandomString(),
                ModelPluralName = GetRandomString(),
                Name = GetRandomString(),
                Description = GetRandomString(),
                Stack = GetRandomString(),
                Language = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Services.Foundations.Templates.Tasks.Task someTask =
                new Core.Models.Services.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(),
                    BranchName = GetRandomString(),
                    Actions = new List<Action>()
                {
                    new Core.Models.Services.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = invalidString,
                        Files = new List<File>(),
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
                key: "Tasks[0].Actions[0].Name",
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
        public async Task ShouldThrowValidationExceptionWhenTemplateTaskActionFileItemsIsInvalidAsync(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                ModelSingularName = GetRandomString(),
                ModelPluralName = GetRandomString(),
                Name = GetRandomString(),
                Description = GetRandomString(),
                Stack = GetRandomString(),
                Language = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Services.Foundations.Templates.Tasks.Task someTask =
                new Core.Models.Services.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(),
                    BranchName = GetRandomString(),
                    Actions = new List<Action>()
                {
                    new Core.Models.Services.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<File>()
                        {
                            new Core.Models.Services.Foundations.Templates.Tasks.Actions.Files.File()
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
        public async Task ShouldThrowValidationExceptionWhenTemplateTaskActionAppendItemsIsInvalidAsync(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                ModelSingularName = GetRandomString(),
                ModelPluralName = GetRandomString(),
                Name = GetRandomString(),
                Description = GetRandomString(),
                Stack = GetRandomString(),
                Language = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Services.Foundations.Templates.Tasks.Task someTask = new Core.Models.Services.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                BranchName = GetRandomString(),
                Actions = new List<Action>()
                {
                    new Core.Models.Services.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<File>()
                        {
                            new File()
                            {
                                Template = GetRandomString(),
                                Target = GetRandomString()
                            },
                        },
                        Appends = new List<Append>()
                        {
                            new Core.Models.Services.Foundations.Templates.Tasks.Actions.Appends.Append()
                            {
                                Target = invalidString,
                                RegexToMatchForAppend = invalidString,
                                ContentToAppend = invalidString
                            }
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
                key: "Actions[0].Appends[0].Target",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Actions[0].Appends[0].RegexToMatch",
                values: "Text is required");

            invalidTemplateException.AddData(
                key: "Actions[0].Appends[0].ContentToAppend",
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
        public async Task ShouldThrowValidationExceptionOnConvertIfTemplateTaskActionExecutionsIsInvalidAsync(
            string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                ModelSingularName = GetRandomString(),
                ModelPluralName = GetRandomString(),
                Name = GetRandomString(),
                Description = GetRandomString(),
                Stack = GetRandomString(),
                Language = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Services.Foundations.Templates.Tasks.Task someTask = new Core.Models.Services.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                BranchName = GetRandomString(),
                Actions = new List<Action>()
                {
                    new Core.Models.Services.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<File>()
                        {
                            new Core.Models.Services.Foundations.Templates.Tasks.Actions.Files.File()
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
