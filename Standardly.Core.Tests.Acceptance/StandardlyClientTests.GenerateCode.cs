// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Standardly.Core.Clients;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Acceptance
{
    public partial class StandardlyClientTests
    {
        [Fact]
        public async Task ShouldGenerateCodeForTemplateAsync()
        {
            //given
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string solutionFolder = Path.Combine(Path.GetDirectoryName(assembly), @"Output");
            string templateDefinitionFileName = "Template.json";

            Dictionary<string, string> replacementDictionary = this.GetReplacementDictionary(
                templateFolder: templateFolderPath,
                nameSingular: "Student",
                namePlural: "Students",
                rootNameSpace: "Standardly",
                solutionFolder: solutionFolder,
                displayName: "Test User",
                gitHubUsername: "test.user@domain.com");

            var standardlyClient = new StandardlyClient(templateFolderPath, templateDefinitionFileName)
            {
                ScriptExecutionIsEnabled = false
            };

            List<Template> templates =
                await standardlyClient.FindAllTemplatesAsync();

            standardlyClient.LogRaised += (date, message, type) =>
            {
                System.Diagnostics.Debug.WriteLine($"{date} - {type} - {message}");
            };

            //when
            await standardlyClient.GenerateCodeAsync(templates, replacementDictionary);

            //then

        }
    }
}
