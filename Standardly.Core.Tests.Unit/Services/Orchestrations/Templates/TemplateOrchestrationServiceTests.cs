// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Appends;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Files;
using Standardly.Core.Models.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Processings.Executions.Exceptions;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Standardly.Core.Models.Processings.Templates.Exceptions;
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
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITemplateGenerationOrchestrationService templateOrchestrationService;

        public TemplateOrchestrationServiceTests()
        {
            this.fileProcessingServiceMock = new Mock<IFileProcessingService>();
            this.executionProcessingServiceMock = new Mock<IExecutionProcessingService>();
            this.templateProcessingServiceMock = new Mock<ITemplateProcessingService>();
            this.templateConfigMock = new Mock<ITemplateConfig>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.templateConfigMock
                .Setup(config => config.TemplateFolderPath).Returns("c:\\Standardly\\Templates");

            this.templateConfigMock
                .Setup(config => config.TemplateDefinitionFileName).Returns("Template.json");

            templateOrchestrationService = new TemplateGenerationOrchestrationService(
                fileProcessingService: fileProcessingServiceMock.Object,
                executionProcessingService: executionProcessingServiceMock.Object,
                templateProcessingService: templateProcessingServiceMock.Object,
                templateConfig: templateConfigMock.Object,
                loggingBroker: loggingBrokerMock.Object);
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

        public static TheoryData TemplateOrchestrationDependencyValidationExceptions()
        {
            string exceptionMessage = GetRandomString();
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>()
            {
                new FileProcessingValidationException(innerException),
                new FileProcessingDependencyValidationException(innerException),
                new ExecutionProcessingValidationException(innerException),
                new ExecutionProcessingDependencyValidationException(innerException),
                new TemplateProcessingValidationException(innerException),
                new TemplateProcessingDependencyValidationException(innerException)
            };
        }

        public static TheoryData TemplateOrchestrationDependencyExceptions()
        {
            string exceptionMessage = GetRandomString();
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>()
            {
                new FileProcessingDependencyException(innerException),
                new FileProcessingServiceException(innerException),
                new TemplateProcessingDependencyException(innerException),
                new TemplateProcessingServiceException(innerException),
                new ExecutionProcessingDependencyException(innerException),
                new ExecutionProcessingServiceException(innerException)
            };
        }

        private static List<Append> CreateAppends(int itemsToGenerate)
        {
            var appends = new List<Append>();

            for (int i = 0; i < itemsToGenerate; i++)
            {
                appends.Add(new Append()
                {
                    Target = GetRandomString(),
                    DoesNotContainContent = GetRandomString(),
                    RegexToMatchForAppend = GetRandomString(),
                    ContentToAppend = GetRandomString(),
                    AppendToBeginning = false,
                    AppendEvenIfContentAlreadyExist = false
                });
            }

            return appends;
        }

        private static List<File> CreateFiles(int itemsToGenerate, bool replaceFiles)
        {
            var files = new List<File>();

            for (int i = 0; i < itemsToGenerate; i++)
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

        private static List<Execution> CreateExecutions(int itemsToGenerate)
        {
            var executions = new List<Execution>();

            for (int i = 0; i < itemsToGenerate; i++)
            {
                executions.Add(new Standardly.Core.Models.Foundations.Executions.Execution()
                {
                    Name = GetRandomString(),
                    Instruction = GetRandomString()
                });
            }

            return executions;
        }

        private static List<Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Action> CreateActions(
            int itemsToGenerate,
            bool replaceFiles)
        {
            var actions = new List<Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Action>();

            for (int i = 0; i < itemsToGenerate; i++)
            {
                actions.Add(new Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Action()
                {
                    Name = GetRandomString(),
                    ExecutionFolder = GetRandomString(),
                    Files = CreateFiles(itemsToGenerate, replaceFiles),
                    Appends = CreateAppends(itemsToGenerate),
                    Executions = CreateExecutions(itemsToGenerate)
                });
            }

            return actions;
        }

        private static List<Core.Models.Foundations.Templates.Tasks.Task> CreateTasks(int itemsToGenerate, bool replaceFiles)
        {
            var tasks = new List<Core.Models.Foundations.Templates.Tasks.Task>();

            for (int i = 0; i < itemsToGenerate; i++)
            {
                tasks.Add(new Core.Models.Foundations.Templates.Tasks.Task()
                {
                    Name = GetRandomString(),
                    Actions = CreateActions(itemsToGenerate, replaceFiles)
                });
            }

            return tasks;
        }

        private static Dictionary<string, string> CreateReplacementDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                dictionary.Add($"${GetRandomString(1)}$", GetRandomString(1));
            }

            dictionary.Add("$previousBranch$", GetRandomString(1));
            dictionary.Add("$basebranch$", GetRandomString(1));

            return dictionary;
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
            new IntRange(min: 2, max: 5).GetValue();

        private static string GetRandomString(int wordCount) =>
            new MnemonicString(wordCount: wordCount).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static List<Template> GetRandomTemplateList(int itemsToGenerate, bool replaceFiles = true)
        {
            return Enumerable.Range(start: 0, count: itemsToGenerate)
                .Select(item =>
                {
                    return CreateRandomTemplate(itemsToGenerate, replaceFiles);
                }).ToList();
        }

        private static Template CreateRandomTemplate(int itemsToGenerate, bool replaceFiles = true) =>
            CreateTemplateFiller(itemsToGenerate, replaceFiles).Create();

        private static Filler<Template> CreateTemplateFiller(int itemsToGenerate, bool replaceFiles = true)
        {
            var filler = new Filler<Template>();
            filler.Setup()
                .OnType<List<Core.Models.Foundations.Templates.Tasks.Task>>().Use(CreateTasks(itemsToGenerate, replaceFiles))
                .OnType<Dictionary<string, string>>().Use(CreateDictionary);

            return filler;
        }
    }
}
