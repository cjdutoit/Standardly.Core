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
            string sourceContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.1.Source.txt"));
            string resultContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.1.Result.txt"));
            string expectedResult = resultContent.Trim();
            string doesNotContainContent = string.Empty;
            bool appendToBeginning = true;
            bool appendEvenIfContentAlreadyExist = false;
            string appendContent = "            services.AddDbContext<StorageBroker>();";

            string regexToMatch = @"(?<=public void ConfigureServices\(IServiceCollection services\)"
                + @"\r\n        \{\r\n)([\S\s]*?)(?=\n        \}\r\n)";

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Trim().Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ShouldNotAppendContentAsync()
        {
            // given
            var assembly = Assembly.GetExecutingAssembly().Location;
            var resourceFolder = Path.Combine(Path.GetDirectoryName(assembly), "Resources");
            string sourceContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.2.Source.txt"));
            string resultContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.2.Result.txt"));
            string doesNotContainContent = string.Empty;
            string expectedResult = resultContent.Trim();
            bool appendToBeginning = true;
            bool appendEvenIfContentAlreadyExist = false;
            string appendContent = "            services.AddDbContext<StorageBroker>();";

            string regexToMatch = @"(?<=public void ConfigureServices\(IServiceCollection services\)"
                + @"\r\n        \{\r\n)([\S\s]*?)(?=\n        \}\r\n)";

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContainContent,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Trim().Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ShouldAppendIfTheDoesNotContainContentIsNotPresentAsync()
        {
            // given
            var assembly = Assembly.GetExecutingAssembly().Location;
            var resourceFolder = Path.Combine(Path.GetDirectoryName(assembly), "Resources");
            string sourceContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.3.Source.txt"));
            string resultContent = File.ReadAllText(Path.Combine(resourceFolder, "Startup.cs.3.Result.txt"));
            string expectedResult = resultContent.Trim();
            bool appendToBeginning = false;
            bool appendEvenIfContentAlreadyExist = false;
            string regexToMatch = @"(?<=public class Startup\r\n    \{\r\n)([\S\s]*?)(?=\n    \}\r\n)";
            string doesNotContain = "private static void AddServices(IServiceCollection services)";
            string appendContent = "        private static void AddServices(IServiceCollection services)\r\n        {\r\n\r\n        }";

            // when
            string actualResult = await this.templateService
                .AppendContentAsync(
                    sourceContent,
                    doesNotContain,
                    regexToMatch,
                    appendContent,
                    appendToBeginning,
                    appendEvenIfContentAlreadyExist);

            // then
            actualResult.Trim().Should().BeEquivalentTo(expectedResult);
        }
    }
}
