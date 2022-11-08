// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Fact]
        public async Task ShouldAppendContentAsync()
        {
            // given
            var assembly = Assembly.GetExecutingAssembly().Location;
            var resourceFolder = Path.Combine(Path.GetDirectoryName(assembly), "Resources");
            string sourceContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.Source.txt"));
            string resultContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.Result.txt"));
            string expectedResult = resultContent.Trim();
            bool appendToBeginning = true;
            bool onlyAppendIfNotPresent = true;
            string appendContent = "            services.AddDbContext<StorageBroker>();";

            string regexToMatch = @"(?<=public void ConfigureServices\(IServiceCollection services\)\r\n        \{\r\n)([\S\s]*?)(?=\n        \}\r\n)";

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(sourceContent, regexToMatch, appendContent, appendToBeginning, onlyAppendIfNotPresent);

            // then
            actualResult.Trim().Should().BeEquivalentTo(expectedResult);
        }
    }
}
