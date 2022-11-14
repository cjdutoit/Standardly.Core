﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        [Fact]
        public void ShouldValidateTransform()
        {
            try
            {
                // given
                Dictionary<string, string> randomReplacementDictionary = CreateReplacementDictionary();
                Dictionary<string, string> inputReplacementDictionary = randomReplacementDictionary;
                string randomStringTemplate = CreateStringTemplate(randomReplacementDictionary);
                string inputStringTemplate = randomStringTemplate;

                // when then
                string transformedTemplate =
                    this.templateService.TransformString(inputStringTemplate, inputReplacementDictionary);

                this.templateService.ValidateTransformation(transformedTemplate);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
