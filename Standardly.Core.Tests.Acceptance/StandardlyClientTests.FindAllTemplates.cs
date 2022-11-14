﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Standardly.Core.Clients;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Acceptance
{
    public partial class StandardlyClientTests
    {
        [Fact]
        public void ShouldFindListOfTemplates()
        {
            //given
            var standardlyClient = new StandardlyClient
            {
                ScriptExecutionIsEnabled = false
            };

            //when
            List<Template> templates = standardlyClient.FindAllTemplates();

            //then
            templates.Count().Should().Be(3);
            templates.Any(template => template.Name == "BROKERS: DateTime Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Logging Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Storage Broker").Should().BeTrue();
        }

        [Fact]
        public void ShouldFindListOfTemplatesWithCustomConfig()
        {
            //given
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";
            var standardlyClient = new StandardlyClient(templateFolderPath, templateDefinitionFileName)
            {
                ScriptExecutionIsEnabled = false
            };

            //when
            List<Template> templates = standardlyClient.FindAllTemplates();

            //then
            templates.Count().Should().Be(3);
            templates.Any(template => template.Name == "BROKERS: DateTime Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Logging Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Storage Broker").Should().BeTrue();
        }
    }
}