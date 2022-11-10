// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Models.Orchestrations.Templates
{
    public class TemplateConfig : ITemplateConfig
    {
        public TemplateConfig(
            string templateFolderPath,
            string templateDefinitionFileName)
        {
            this.TemplateFolderPath = templateFolderPath;
            this.TemplateDefinitionFileName = templateDefinitionFileName;
        }

        public string TemplateFolderPath { get; private set; }
        public string TemplateDefinitionFileName { get; private set; }
    }
}
