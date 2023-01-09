// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Models.Services.Orchestrations.TemplateGenerations
{
    public interface ITemplateConfig
    {
        string TemplateFolderPath { get; }
        string TemplateDefinitionFileName { get; }
    }
}
