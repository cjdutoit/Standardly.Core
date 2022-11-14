// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Files;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnConvertStringToTemplateIfContentIsNullOrEmpty(
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(content);

            var actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnConvertStringToTemplateIfTemplateIsInvalid(
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            var actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnConvertIfTemplateTasksIsInvalid(string invalidString)
        {
            // given
            Template someTemplate = new Template()
            {
                Name = GetRandomString(),
                Description = GetRandomString(),
                TemplateType = GetRandomString(),
                ProjectsRequired = GetRandomString()
            };

            Core.Models.Foundations.Templates.Tasks.Task someTask = new Core.Models.Foundations.Templates.Tasks.Task()
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            TemplateValidationException actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnConvertIfTemplateTaskActionsIsInvalid(
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

            Core.Models.Foundations.Templates.Tasks.Task someTask = new Core.Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                BranchName = GetRandomString(),
                Actions = new List<Core.Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Core.Models.Foundations.Templates.Tasks.Actions.Action()
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            TemplateValidationException actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionWhenTemplateTaskActionFileItemsIsInvalid(
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

            Core.Models.Foundations.Templates.Tasks.Task someTask =
                new Core.Models.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(),
                    BranchName = GetRandomString(),
                    Actions = new List<Core.Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Core.Models.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<File>()
                        {
                            new Core.Models.Foundations.Templates.Tasks.Actions.Files.File()
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            TemplateValidationException actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionWhenTemplateTaskActionAppendItemsIsInvalid(
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

            Core.Models.Foundations.Templates.Tasks.Task someTask = new Core.Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                BranchName = GetRandomString(),
                Actions = new List<Core.Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Core.Models.Foundations.Templates.Tasks.Actions.Action()
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
                        Appends = new List<Core.Models.Foundations.Templates.Tasks.Actions.Appends.Append>()
                        {
                            new Core.Models.Foundations.Templates.Tasks.Actions.Appends.Append()
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            TemplateValidationException actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowValidationExceptionOnConvertIfTemplateTaskActionExecutionsIsInvalid(
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

            Core.Models.Foundations.Templates.Tasks.Task someTask = new Core.Models.Foundations.Templates.Tasks.Task()
            {
                Name = GetRandomString(),
                BranchName = GetRandomString(),
                Actions = new List<Core.Models.Foundations.Templates.Tasks.Actions.Action>()
                {
                    new Core.Models.Foundations.Templates.Tasks.Actions.Action()
                    {
                        Name = GetRandomString(),
                        Files = new List<File>()
                        {
                            new Core.Models.Foundations.Templates.Tasks.Actions.Files.File()
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
            Action convertStringToTemplateAction = () =>
                this.templateService.ConvertStringToTemplate(inputStringTemplate);

            TemplateValidationException actualException =
                Assert.Throws<TemplateValidationException>(convertStringToTemplateAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTemplateValidationException);
        }
    }
}
