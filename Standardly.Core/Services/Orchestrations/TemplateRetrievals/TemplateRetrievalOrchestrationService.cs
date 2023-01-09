// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Templates;
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

        public ValueTask<List<Template>> FindAllTemplatesAsync(string templateFolderPath, string templateDefinitionFileName) =>
            TryCatch(async () =>
            {
                ValidateFindTemplateArguments(templateFolderPath, templateDefinitionFileName);

                List<Template> templates = new List<Template>();

                var fileList = await this.fileProcessingService
                    .RetrieveListOfFilesAsync(
                        path: templateFolderPath,
                        searchPattern: templateDefinitionFileName);

                foreach (string file in fileList)
                {
                    try
                    {
                        string rawTemplate = this.fileProcessingService.ReadFromFileAsync(file).Result;

                        Template template = await this.templateProcessingService
                            .ConvertStringToTemplateAsync(rawTemplate);

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
