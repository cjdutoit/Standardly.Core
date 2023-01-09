// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Services.Foundations.Templates;

namespace Standardly.Core.Services.Processings.Templates
{
    public partial class TemplateProcessingService : ITemplateProcessingService
    {
        private readonly ITemplateService templateService;

        public TemplateProcessingService(ITemplateService templateService)
        {
            this.templateService = templateService;
        }

        public ValueTask<Template> ConvertStringToTemplateAsync(string content) =>
            TryCatch(async () =>
            {
                ValidateConvertStringToTemplate(content);

                return await this.templateService.ConvertStringToTemplateAsync(content);
            });

        public ValueTask<Template> TransformTemplateAsync(Template template) =>
                TryCatch(async () =>
                {
                    ValidateTransformTemplate(template, replacementDictionary: template?.ReplacementDictionary);

                    var transformedStringTemplate = await this.templateService
                        .TransformStringAsync(
                            content: template.RawTemplate,
                            replacementDictionary: template.ReplacementDictionary);

                    await this.templateService.ValidateTransformationAsync(transformedStringTemplate);

                    var transformedTemplate = await this.templateService
                        .ConvertStringToTemplateAsync(transformedStringTemplate);

                    return transformedTemplate;
                });

        public ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary) =>
                TryCatch(async () =>
                {
                    ValidateTransformString(content, replacementDictionary);

                    var transformedStringTemplate =
                        await this.templateService.TransformStringAsync(content, replacementDictionary);

                    await this.templateService.ValidateTransformationAsync(transformedStringTemplate);

                    return transformedStringTemplate;
                });

        public ValueTask<string> AppendContentAsync(
            string sourceContent,
            string doesNotContainContent,
            string regexToMatchForAppend,
            string appendContent,
            bool appendToBeginning,
            bool appendEvenIfContentAlreadyExist) =>
                TryCatch(async () =>
                {
                    ValidateAppendContent(sourceContent, regexToMatchForAppend, appendContent);

                    return await this.templateService
                        .AppendContentAsync(
                            sourceContent,
                            doesNotContainContent,
                            regexToMatchForAppend,
                            appendContent,
                            appendToBeginning,
                            appendEvenIfContentAlreadyExist);
                });
    }
}
