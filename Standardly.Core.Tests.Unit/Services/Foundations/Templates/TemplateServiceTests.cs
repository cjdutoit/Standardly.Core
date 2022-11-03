﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Services.Foundations.Templates;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        private readonly Mock<IFileBroker> fileBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITemplateService templateService;

        public TemplateServiceTests()
        {
            this.fileBrokerMock = new Mock<IFileBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.templateService = new TemplateService(
                fileBroker: fileBrokerMock.Object, loggingBroker: loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string CreateStringTemplate(Dictionary<string, string> dictionary)
        {
            var stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> item in dictionary)
            {
                stringBuilder.Append($"{item.Key} {GetRandomString()} ");
            }

            return stringBuilder.ToString();
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

        private static string SerializeTemplate(Template template) =>
            JsonConvert.SerializeObject(template);

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static string GetRandomString(int wordCount) =>
            new MnemonicString(wordCount: wordCount).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

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

        private static List<Models.Foundations.Templates.Tasks.Actions.Files.File> CreateListOfFileItems()
        {
            List<Models.Foundations.Templates.Tasks.Actions.Files.File> list =
                new List<Models.Foundations.Templates.Tasks.Actions.Files.File>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Models.Foundations.Templates.Tasks.Actions.Files.File()
                {
                    Template = GetRandomString(1),
                    Target = GetRandomString(1),
                    Replace = true
                });
            }

            return list;
        }

        private static List<Models.Foundations.Templates.Tasks.Actions.Action> CreateListOfActions()
        {
            List<Models.Foundations.Templates.Tasks.Actions.Action> list =
                new List<Models.Foundations.Templates.Tasks.Actions.Action>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Models.Foundations.Templates.Tasks.Actions.Action()
                {
                    Name = GetRandomString(1),
                    ExecutionFolder = GetRandomString(1),
                    Files = CreateListOfFileItems(),
                    Executions = CreateListOfExecutions()
                });
            }

            return list;
        }

        private static List<Models.Foundations.Templates.Tasks.Task> CreateListOfTasks()
        {
            List<Models.Foundations.Templates.Tasks.Task> list =
                new List<Models.Foundations.Templates.Tasks.Task>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                list.Add(new Models.Foundations.Templates.Tasks.Task()
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
                .OnType<List<Models.Foundations.Templates.Tasks.Task>>().Use(CreateListOfTasks);

            return filler;
        }
    }
}