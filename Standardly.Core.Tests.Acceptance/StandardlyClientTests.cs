// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Standardly.Core.Clients;
using Standardly.Core.Models.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Acceptance
{
    public class StandardlyClientTests
    {
        [Fact]
        public async Task ShouldFindListOfTemplatesAsync()
        {
            //given
            var standardlyClient = new StandardlyClient();

            //when
            List<Template> templates = await standardlyClient.FindAllTemplatesAsync();

            //then
            templates.Count().Should().Be(3);
            templates.Any(template => template.Name == "BROKERS: DateTime Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Logging Broker").Should().BeTrue();
            templates.Any(template => template.Name == "BROKERS: Storage Broker").Should().BeTrue();
        }
    }
}
