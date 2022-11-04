// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Files;
using Standardly.Core.Models.Orchestrations.Templates;
using Standardly.Core.Services.Orchestrations.Templates;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationServiceTests
    {
        private readonly Mock<IFileProcessingService> fileProcessingServiceMock;
        private readonly Mock<IExecutionProcessingService> executionProcessingServiceMock;
        private readonly Mock<ITemplateProcessingService> templateProcessingServiceMock;
        private readonly Mock<ITemplateConfig> templateConfigMock;
        private readonly ITemplateOrchestrationService templateOrchestrationService;

        public TemplateOrchestrationServiceTests()
        {
            this.fileProcessingServiceMock = new Mock<IFileProcessingService>();
            this.executionProcessingServiceMock = new Mock<IExecutionProcessingService>();
            this.templateProcessingServiceMock = new Mock<ITemplateProcessingService>();
            this.templateConfigMock = new Mock<ITemplateConfig>();

            this.templateConfigMock
                .Setup(config => config.TemplateFolder).Returns("c:\\Standardly\\Templates");

            this.templateConfigMock
                .Setup(config => config.TemplateFolder).Returns("Template.json");

            templateOrchestrationService = new TemplateOrchestrationService(
                fileProcessingService: fileProcessingServiceMock.Object,
                executionProcessingService: executionProcessingServiceMock.Object,
                templateProcessingService: templateProcessingServiceMock.Object,
                templateConfig: templateConfigMock.Object);
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

        public static TheoryData FindAllTemplateOrchestrationTemplatesDependencyValidationExceptions()
        {
            string exceptionMessage = GetRandomString();
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>()
            {
                new FileValidationException(innerException),
                new FileDependencyValidationException(innerException),
            };
        }

        public static TheoryData FindAllTemplateOrchestrationDependencyExceptions()
        {
            string exceptionMessage = GetRandomString();
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>()
            {
                new FileDependencyException(innerException),
                new FileServiceException(innerException),
            };
        }

        private static List<Append> CreateAppends(int numberOfFileItems)
        {
            var appends = new List<Append>();

            for (int i = 0; i < numberOfFileItems; i++)
            {
                appends.Add(new Append()
                {
                    Target = GetRandomString(),
                    RegexToMatch = GetRandomString(),
                    ContentToAppend = GetRandomString(),
                    AppendToTop = false
                });
            }

            return appends;
        }

        private static List<File> CreateFiles(int numberOfFileItems, bool replaceFiles)
        {
            var files = new List<File>();

            for (int i = 0; i < numberOfFileItems; i++)
            {
                files.Add(new File()
                {
                    Replace = replaceFiles,
                    Template = GetRandomString(),
                    Target = GetRandomString()
                });
            }

            return files;
        }

        private static List<Execution> CreateExecutions(int numberOfExecutions)
        {
            var executions = new List<Execution>();

            for (int i = 0; i < numberOfExecutions; i++)
            {
                executions.Add(new Models.Foundations.Executions.Execution()
                {
                    Name = GetRandomString(),
                    Instruction = GetRandomString()
                });
            }

            return executions;
        }

        private static List<Models.Foundations.Templates.Tasks.Actions.Action> CreateActions(
            int numberOfActions,
            bool replaceFiles)
        {
            var actions = new List<Models.Foundations.Templates.Tasks.Actions.Action>();

            for (int i = 0; i < numberOfActions; i++)
            {
                actions.Add(new Models.Foundations.Templates.Tasks.Actions.Action()
                {
                    Name = GetRandomString(),
                    ExecutionFolder = GetRandomString(),
                    Files = CreateFiles(2, replaceFiles),
                    Appends = CreateAppends(2),
                    Executions = CreateExecutions(2)
                });
            }

            return actions;
        }

        private static List<Models.Foundations.Templates.Tasks.Task> CreateTasks(int numberOfTasks, bool replaceFiles)
        {
            var tasks = new List<Models.Foundations.Templates.Tasks.Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                tasks.Add(new Models.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(),
                    Actions = CreateActions(2, replaceFiles)
                });
            }

            return tasks;
        }

        private static Dictionary<string, string> CreateDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                dictionary.Add(GetRandomString(1), GetRandomString());
            }

            return dictionary;
        }

        private static List<string> GetRandomStringList()
        {
            return Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item =>
                {
                    return GetRandomString();
                }).ToList();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString(int wordCount) =>
            new MnemonicString(wordCount: wordCount).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Template CreateRandomTemplate(bool replaceFiles = true) =>
            CreateTemplateFiller(replaceFiles).Create();

        private static Filler<Template> CreateTemplateFiller(bool replaceFiles = true)
        {
            var filler = new Filler<Template>();
            filler.Setup()
                .OnType<List<Models.Foundations.Templates.Tasks.Task>>().Use(CreateTasks(2, replaceFiles))
                .OnType<Dictionary<string, string>>().Use(CreateDictionary);

            return filler;
        }
    }
}
