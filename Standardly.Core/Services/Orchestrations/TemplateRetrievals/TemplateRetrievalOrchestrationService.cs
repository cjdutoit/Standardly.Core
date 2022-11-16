// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationService : ITemplateRetrievalOrchestrationService
    {
        public bool ScriptExecutionIsEnabled { get; set; } = true;
        private readonly IFileProcessingService fileProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;
        private readonly ITemplateConfig templateConfig;
        private readonly ILoggingBroker loggingBroker;

        public TemplateRetrievalOrchestrationService(
            IFileProcessingService fileProcessingService,
            ITemplateProcessingService templateProcessingService,
            ITemplateConfig templateConfig,
            ILoggingBroker loggingBroker)
        {
            this.fileProcessingService = fileProcessingService;
            this.templateProcessingService = templateProcessingService;
            this.templateConfig = templateConfig;
            this.loggingBroker = loggingBroker;
        }

        public List<Template> FindAllTemplates() =>
            TryCatch(() =>
            {
                List<Template> templates = new List<Template>();

                var fileList = this.fileProcessingService
                    .RetrieveListOfFiles(
                    this.templateConfig.TemplateFolderPath,
                    this.templateConfig.TemplateDefinitionFileName);

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = this.fileProcessingService.ReadFromFile(file);

                        Template template = this.templateProcessingService
                            .ConvertStringToTemplate(rawTemplate);

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
