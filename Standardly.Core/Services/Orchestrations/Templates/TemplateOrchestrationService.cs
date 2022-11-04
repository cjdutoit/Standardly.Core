// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.Templates;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.Templates
{
    public partial class TemplateOrchestrationService : ITemplateOrchestrationService
    {
        private readonly IFileProcessingService fileProcessingService;
        private readonly IExecutionProcessingService executionProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;
        private readonly ITemplateConfig templateConfig;

        public TemplateOrchestrationService(
            IFileProcessingService fileProcessingService,
            IExecutionProcessingService executionProcessingService,
            ITemplateProcessingService templateProcessingService,
            ITemplateConfig templateConfig)
        {
            this.fileProcessingService = fileProcessingService;
            this.executionProcessingService = executionProcessingService;
            this.templateProcessingService = templateProcessingService;
            this.templateConfig = templateConfig;
        }

        public ValueTask<List<Template>> FindAllTemplatesAsync() =>
            TryCatchAsync(async () =>
            {
                List<Template> templates = new List<Template>();

                var fileList = await this.fileProcessingService
                    .RetrieveListOfFilesAsync(
                    this.templateConfig.TemplateFolder,
                    this.templateConfig.TemplateDefinitionFile);

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = await this.fileProcessingService.ReadFromFileAsync(file);
                        Template template = await this.templateProcessingService.ConvertStringToTemplateAsync(rawTemplate);
                        templates.Add(template);
                    }
                    catch (Exception)
                    {
                    }
                }

                return templates;
            });

    }
}
