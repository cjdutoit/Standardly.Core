// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Foundations.Templates.EntityModels;

namespace Standardly.Core.Models.Orchestrations
{
    public class TemplateGenerationInfo
    {
        public List<Template> Templates { get; set; }
        public Dictionary<string, string> ReplacementDictionary { get; set; }
        //public List<EntityModel> EntityModelDefinition { get; set; }
    }
}
