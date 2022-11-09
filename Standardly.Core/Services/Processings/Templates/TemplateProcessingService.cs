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

        public ValueTask<Template> TransformTemplateAsync(
            Template template,
            Dictionary<string, string> replacementDictionary) =>
                TryCatchAsync(async () =>
                {
                    ValidateTransformTemplate(template, replacementDictionary);

                    var transformedStringTemplate = await this.templateService
                        .TransformStringAsync(template.RawTemplate, replacementDictionary);

                    await this.templateService.ValidateTransformationAsync(transformedStringTemplate);

                    var transformedTemplate = await this.templateService
                        .ConvertStringToTemplateAsync(transformedStringTemplate);

                    return transformedTemplate;
                });

        public ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary) =>
                TryCatchAsync(async () =>
                {
                    ValidateTransformString(content, replacementDictionary);

                    var transformedStringTemplate =
                        await this.templateService.TransformStringAsync(content, replacementDictionary);

                    await this.templateService.ValidateTransformationAsync(transformedStringTemplate);

                    return transformedStringTemplate;
                });

        public ValueTask<string> AppendContentAsync(
            string sourceContent,
            string regexToMatch,
            string appendContent,
            bool appendToBeginning = false,
            bool onlyAppendIfNotPresent = true) =>
                TryCatchAsync(async () =>
                {
                    ValidateAppendContent(sourceContent, regexToMatch, appendContent);

                    return await this.templateService
                        .AppendContentAsync(sourceContent, regexToMatch, appendContent, onlyAppendIfNotPresent);
                });
    }
}
