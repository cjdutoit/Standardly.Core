// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Newtonsoft.Json;
using Standardly.Core.Models.Services.Foundations.Executions;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Foundations.Templates.Exceptions;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Files;
using Standardly.Core.Services.Foundations.Templates;
using Standardly.Core.Services.Processings.Templates;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        private readonly Mock<ITemplateService> templateServiceMock;
        private readonly ITemplateProcessingService templateProcessingService;

        public TemplateProcessingServiceTests()
        {
            this.templateServiceMock = new Mock<ITemplateService>();

            this.templateProcessingService = new TemplateProcessingService(
                templateService: this.templateServiceMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData InvalidCharacters()
        {
            return new TheoryData<char>()
            {
                new char(),
                ' ',
            };
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TemplateValidationException(innerException),
                new TemplateDependencyValidationException(innerException)
            };
        }

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new TemplateDependencyException(innerException),
                new TemplateServiceException(innerException)
            };
        }

        private static Dictionary<string, string> CreateReplacementDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                dictionary.Add($"${GetRandomString(1)}$", GetRandomString(1));
            }

            return dictionary;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int wordCount) =>
            new MnemonicString(wordCount: wordCount).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<Execution> CreateListOfExecutions()
        {
            List<Execution> list = new List<Execution>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Execution()
                {
                    Name = GetRandomString(1),
                    Instruction = GetRandomString(1)
                });
            }

            return list;
        }

        private static List<Append> CreateListOfAppends()
        {
            List<Append> list =
                new List<Append>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Append()
                {
                    Target = GetRandomString(1),
                    DoesNotContainContent = GetRandomString(1),
                    RegexToMatchForAppend = GetRandomString(1),
                    ContentToAppend = GetRandomString(1),
                    AppendToBeginning = false,
                    AppendEvenIfContentAlreadyExist = false
                });
            }

            return list;
        }

        private static List<File> CreateListOfFileItems()
        {
            List<File> list =
                new List<File>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Files.File()
                {
                    Template = GetRandomString(1),
                    Target = GetRandomString(1),
                    Replace = true
                });
            }

            return list;
        }

        private static List<Core.Models.Services.Foundations.Templates.Tasks.Actions.Action> CreateListOfActions()
        {
            List<Core.Models.Services.Foundations.Templates.Tasks.Actions.Action> list =
                new List<Core.Models.Services.Foundations.Templates.Tasks.Actions.Action>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Standardly.Core.Models.Services.Foundations.Templates.Tasks.Actions.Action()
                {
                    Name = GetRandomString(1),
                    ExecutionFolder = GetRandomString(1),
                    Files = CreateListOfFileItems(),
                    Appends = CreateListOfAppends(),
                    Executions = CreateListOfExecutions()
                });
            }

            return list;
        }

        private static List<Task> CreateListOfTasks()
        {
            List<Task> list =
                new List<Task>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Core.Models.Services.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(1),
                    Actions = CreateListOfActions()
                });
            }

            return list;
        }

        private static List<string> CreateListOfStrings()
        {
            List<string> list = new List<string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(GetRandomString(1));
            }

            return list;
        }

        private static string SerializeTemplate(Template template) =>
            JsonConvert.SerializeObject(template);

        private static Template CreateRandomTemplate()
        {
            Template template = CreateTemplateFiller().Create();
            template.RawTemplate = SerializeTemplate(template);

            return template;
        }

        private static Filler<Template> CreateTemplateFiller()
        {
            var filler = new Filler<Template>();
            filler.Setup()
                .OnType<List<string>>().Use(CreateListOfStrings)
                .OnType<List<Task>>().Use(CreateListOfTasks);

            return filler;
        }
    }
}
