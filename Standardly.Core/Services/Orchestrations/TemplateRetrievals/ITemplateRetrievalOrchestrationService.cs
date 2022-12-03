// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public interface ITemplateRetrievalOrchestrationService
    {
        List<Template> FindAllTemplates(string templateFolderPath, string templateDefinitionFileName);
    }
}
