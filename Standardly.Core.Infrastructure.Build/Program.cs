using Standardly.Core.Services;

var scriptGenerationService = new ScriptGenerationService();
scriptGenerationService.GenerateBuildScript();
scriptGenerationService.GenerateProvisionScript();