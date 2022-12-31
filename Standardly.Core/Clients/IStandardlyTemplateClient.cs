// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Clients
{
    public interface IStandardlyTemplateClient
    {
        ValueTask<List<Template>> FindAllTemplatesAsync(string templateFolderPath, string templateDefinitionFileName);
    }
}
