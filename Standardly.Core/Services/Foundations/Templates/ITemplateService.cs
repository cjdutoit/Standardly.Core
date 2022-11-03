﻿// ---------------------------------------------------------------
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
        ValueTask<string> TransformString(string content, Dictionary<string, string> replacementDictionary);
        ValueTask ValidateTransformation(string content, char tagCharacter);
        ValueTask<Template> ConvertStringToTemplate(string content);
    }
}
