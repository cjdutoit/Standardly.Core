// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Models.Orchestrations.Templates
{
    public interface ITemplateConfig
    {
        string TemplateFolder { get; set; }
        string TemplateDefinitionFile { get; set; }
    }
}
