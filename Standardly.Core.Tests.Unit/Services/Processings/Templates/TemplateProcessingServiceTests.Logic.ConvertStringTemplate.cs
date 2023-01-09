// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standardly.Core.Models.Services.Foundations.Templates;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Processings.Templates
{
    public partial class TemplateProcessingServiceTests
    {
        [Fact]
        public async Task ShouldConvertStringToTemplateExecutionAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputString = randomString;
            Template randomTemplate = CreateRandomTemplate();
            Template expectedTemplate = randomTemplate;

            this.templateServiceMock.Setup(service =>
                service.ConvertStringToTemplateAsync(inputString))
                    .ReturnsAsync(expectedTemplate);

            // when
            Template actualTemplate = await this.templateProcessingService
                .ConvertStringToTemplateAsync(inputString);

            // then
            actualTemplate.Should().BeEquivalentTo(expectedTemplate);

            this.templateServiceMock.Verify(service =>
                service.ConvertStringToTemplateAsync(inputString),
                    Times.Once());

            this.templateServiceMock.VerifyNoOtherCalls();
        }
    }
}
