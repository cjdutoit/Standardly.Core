// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Standardly.Core.Clients;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.EntityModels;
using Standardly.Core.Models.Orchestrations;
using Standardly.Core.Tests.Acceptance.Models;
using Xunit;

namespace Standardly.Core.Tests.Acceptance
{
    public partial class StandardlyGenerationClientTests
    {
        [Fact]
        public void ShouldGenerateCodeForTemplate()
        {
            //given
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string solutionFolder = Path.Combine(Path.GetDirectoryName(assembly), @"ActualOutput");
            string comparisonFolder = Path.Combine(Path.GetDirectoryName(assembly), @"ExpectedOutput");
            string templateDefinitionFileName = "Template.json";

            List<FileLocations> files = new List<FileLocations>();
            files.AddRange(
                this.GetFileLocations(
                    comparisonFolder,
                    solutionFolder,
                    "Standardly\\Startup.cs",
                    "Standardly\\Brokers\\Datetimes\\IDateTimeBroker.cs",
                    "Standardly\\Brokers\\Datetimes\\DateTimeBroker.cs",
                    "Standardly\\Brokers\\Loggings\\ILoggingBroker.cs",
                    "Standardly\\Brokers\\Loggings\\LoggingBroker.cs",
                    "Standardly\\Brokers\\Storages\\IStorageBroker.cs",
                    "Standardly\\Brokers\\Storages\\IStorageBroker.Students.cs",
                    "Standardly\\Brokers\\Storages\\StorageBroker.cs",
                    "Standardly\\Brokers\\Storages\\StorageBroker.Students.cs",
                    "Standardly\\Brokers\\Storages\\StorageBroker.Students.Configurations.cs",
                    "Standardly\\Brokers\\Storages\\StorageBroker.Students.SeedData.cs"));

            Dictionary<string, string> replacementDictionary = this.GetReplacementDictionary(
                templateFolder: templateFolderPath,
                nameSingular: "Student",
                namePlural: "Students",
                rootNameSpace: "Standardly",
                solutionFolder: solutionFolder,
                displayName: "Test User",
                gitHubUsername: "test.user@domain.com");

            List<EntityModel> entityModelDefinition = new List<EntityModel>();

            var standardlyTemplateClient =
                new StandardlyTemplateClient(templateFolderPath, templateDefinitionFileName);

            List<Template> templates =
                standardlyTemplateClient.FindAllTemplates();

            TemplateGenerationInfo templateGenerationInfo =
                new TemplateGenerationInfo
                {
                    Templates = templates,
                    ReplacementDictionary = replacementDictionary,
                    EntityModelDefinition = entityModelDefinition
                };

            var standardlyGenerationClient = new StandardlyGenerationClient()
            {
                ScriptExecutionIsEnabled = false
            };


            standardlyGenerationClient.LogRaised += (date, message, type) =>
            {
                System.Diagnostics.Debug.WriteLine($"{date} - {type} - {message}");
            };

            //when
            standardlyGenerationClient.GenerateCode(templateGenerationInfo);

            //then
            foreach (FileLocations fileLocations in files)
            {
                var actualResult = File.ReadAllText(fileLocations.ActualFilePath);
                var expectedResult = File.ReadAllText(fileLocations.ExpectedFilePath);
                actualResult.Should().BeEquivalentTo(expectedResult);
            }
        }

        private List<FileLocations> GetFileLocations(
            string comparisonFolder,
            string solutionFolder,
            params string[] relativePaths)
        {
            var locationList = new List<FileLocations>();

            foreach (string relativePath in relativePaths)
            {
                var locations = new FileLocations
                {
                    ExpectedFilePath = Path.Combine(comparisonFolder, relativePath),
                    ActualFilePath = Path.Combine(solutionFolder, relativePath)
                };
                locationList.Add(locations);
            }

            return locationList;
        }
    }
}
