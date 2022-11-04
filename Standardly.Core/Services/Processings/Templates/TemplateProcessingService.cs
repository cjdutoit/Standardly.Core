// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Services.Foundations.Templates;

namespace Standardly.Core.Services.Processings.Templates
{
    public partial class TemplateProcessingService : ITemplateProcessingService
    {
        private readonly ITemplateService templateService;
        private readonly ILoggingBroker loggingBroker;

        public TemplateProcessingService(
            ITemplateService templateService,
            ILoggingBroker loggingBroker)
        {
            this.templateService = templateService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Template> ConvertStringToTemplateAsync(string content) =>
            TryCatchAsync(async () =>
            {
                ValidateConvertStringToTemplate(content);

                return await this.templateService.ConvertStringToTemplateAsync(content);
            });

        public async ValueTask<Template> TransformTemplateAsync(
            Template template,
            Dictionary<string, string> replacementDictionary,
            char tagCharacter)
        {
            var transformedStringTemplate = await this.templateService
                .TransformStringAsync(template.RawTemplate, replacementDictionary);

            await this.templateService
                .ValidateTransformationAsync(transformedStringTemplate, tagCharacter);

            var transformedTemplate = await this.templateService
                .ConvertStringToTemplateAsync(transformedStringTemplate);

            return transformedTemplate;
        }
    }
}
