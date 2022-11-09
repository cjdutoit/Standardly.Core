// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Foundations.Templates
{
    public interface ITemplateService
    {
        ValueTask<string> TransformStringAsync(string content, Dictionary<string, string> replacementDictionary);
        ValueTask ValidateTransformationAsync(string content);
        ValueTask<Template> ConvertStringToTemplateAsync(string content);

        ValueTask<string> AppendContentAsync(
            string sourceContent,
            string regexToMatchForAppend,
            string contentToAppend,
            bool appendToBeginning = false,
            bool appendEvenIfContentAlreadyExist = false);
    }
}
