// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;

namespace Standardly.Core.Services.Orchestrations.TemplateRetrievals
{
    public partial class TemplateRetrievalOrchestrationService : ITemplateRetrievalOrchestrationService
    {
        private readonly IFileProcessingService fileProcessingService;
        private readonly ITemplateProcessingService templateProcessingService;

        public TemplateRetrievalOrchestrationService(
            IFileProcessingService fileProcessingService,
            ITemplateProcessingService templateProcessingService)
        {
            this.fileProcessingService = fileProcessingService;
            this.templateProcessingService = templateProcessingService;
        }

        public List<Template> FindAllTemplates(string templateFolderPath, string templateDefinitionFileName) =>
            TryCatch(() =>
            {
                ValidateFindTemplateArguments(templateFolderPath, templateDefinitionFileName);

                List<Template> templates = new List<Template>();

                var fileList = this.fileProcessingService
                    .RetrieveListOfFilesAsync(
                        path: templateFolderPath,
                        searchPattern: templateDefinitionFileName).Result;

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = this.fileProcessingService.ReadFromFileAsync(file).Result;

                        Template template = this.templateProcessingService
                            .ConvertStringToTemplate(rawTemplate);

                        templates.Add(template);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                return templates;
            });
    }
}
