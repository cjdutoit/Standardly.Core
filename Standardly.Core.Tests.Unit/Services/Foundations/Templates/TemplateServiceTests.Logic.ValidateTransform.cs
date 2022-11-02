// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Fact]
        public async Task ShouldValidateTransformAsync()
        {
            try
            {
                // given
                Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
                Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
                string randomStringTemplate = CreateStringTemplate(randomReplacementDictionary);
                string inputStringTemplate = randomStringTemplate;
                char replacementTag = '$';

                // when then
                string actualTemplate =
                    await this.templateService.TransformString(inputStringTemplate, inputReplacementDictionary);

                await this.templateService.ValidateTransformation(actualTemplate, replacementTag);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
