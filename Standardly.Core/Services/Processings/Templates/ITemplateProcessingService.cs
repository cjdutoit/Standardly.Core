﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Processings.Templates
{
    public interface ITemplateProcessingService
    {
        ValueTask<Template> ConvertStringToTemplateAsync(string content);

        ValueTask<Template> TransformTemplateAsync(
            Template template,
            Dictionary<string, string> replacementDictionary);

        ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary);

        ValueTask<string> AppendContentAsync(
            string sourceContent,
            string regexToMatch,
            string appendContent,
            bool appendToBeginning = false,
            bool onlyAppendIfNotPresent = true);
    }
}
