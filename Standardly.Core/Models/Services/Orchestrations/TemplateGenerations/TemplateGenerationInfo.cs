// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Foundations.Templates;
using Standardly.Core.Models.Services.Foundations.Templates.EntityModels;

namespace Standardly.Core.Models.Services.Orchestrations.TemplateGenerations
{
    public class TemplateGenerationInfo
    {
        public Processed Processed { get; set; }
        public List<Template> Templates { get; set; }
        public Dictionary<string, string> ReplacementDictionary { get; set; }
        public List<EntityModel> EntityModelDefinition { get; set; }
        public bool ScriptExecutionIsEnabled { get; set; } = true;
    }
}
