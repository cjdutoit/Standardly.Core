// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService : ITemplateService
    {
        private readonly IFileBroker fileBroker;
        private readonly ILoggingBroker loggingBroker;

        public TemplateService(IFileBroker fileBroker, ILoggingBroker loggingBroker)
        {
            this.fileBroker = fileBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> TransformStringAsync(
            string content,
            Dictionary<string, string> replacementDictionary) =>
                TryCatchAsync(async () =>
                {
                    ValidateTransformString(content, replacementDictionary);

                    string template = content;

                    if (replacementDictionary != null && replacementDictionary.Any())
                    {
                        foreach (var replacement in replacementDictionary)
                        {
                            template = template.Replace(replacement.Key, replacement.Value);
                        }
                    }

                    return await Task.FromResult(template);
                });

        public ValueTask ValidateTransformationAsync(string content) =>
            TryCatchAsync(async () =>
            {
                ValidateTransformationArguments(content);

                await Task.Run(() => CheckAllTagsHasBeenReplaced(content));
            });

        public ValueTask<Template> ConvertStringToTemplateAsync(string content) =>
            TryCatchAsync(async () =>
            {
                ValidateConvertStringToTemplateArguments(content);

                Template template =
                    JsonConvert.DeserializeObject<Template>(content);

                template.RawTemplate = content;
                ValidateTemplate(template);

                return await Task.FromResult(template);
            });

        public ValueTask<string> AppendContent(
            string sourceContent,
            string regexToMatch,
            string appendContent,
            bool appendToBeginning = false,
            bool onlyAppendIfNotPresent = true)
        {
            throw new System.NotImplementedException();
        }
    }
}
