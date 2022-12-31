// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public interface ITemplateRetrievalOrchestrationService
    {
        ValueTask<List<Template>> FindAllTemplatesAsync(string templateFolderPath, string templateDefinitionFileName);
    }
}
