// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Clients;
using Standardly.Core.Models.Services.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Acceptance
{
    public partial class StandardlyTemplateClientTests
    {
        [Fact]
        public async Task ShouldFindListOfTemplates()
        {
            //given
            var standardlyTemplateClient = new StandardlyTemplateClient();

            //when
            List<Template> templates = await standardlyTemplateClient.FindAllTemplatesAsync();

            //then
            templates.Count().Should().Be(3);
            templates.Any(template => template.Name == "BROKERS: DateTime Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Logging Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Storage Broker").Should().BeTrue();
        }

        [Fact]
        public async Task ShouldFindListOfTemplatesWithCustomPath()
        {
            //given
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";

            var standardlyTemplateClient =
                new StandardlyTemplateClient();

            //when
            List<Template> templates = await standardlyTemplateClient
                .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFileName);

            //then
            templates.Count().Should().Be(3);
            templates.Any(template => template.Name == "BROKERS: DateTime Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Logging Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Storage Broker").Should().BeTrue();
        }
    }
}
