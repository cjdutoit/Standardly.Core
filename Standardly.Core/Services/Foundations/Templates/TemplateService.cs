// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Foundations.Templates;

namespace Standardly.Core.Services.Foundations.Templates
{
    public partial class TemplateService : ITemplateService
    {
        private readonly IFileBroker fileBroker;
        private readonly IRegularExpressionBroker regularExpressionBroker;
        private readonly ILoggingBroker loggingBroker;

        public TemplateService(
            IFileBroker fileBroker,
            IRegularExpressionBroker regularExpressionBroker,
            ILoggingBroker loggingBroker)
        {
            this.fileBroker = fileBroker;
            this.regularExpressionBroker = regularExpressionBroker;
            this.loggingBroker = loggingBroker;
        }

        public string TransformString(
            string content,
            Dictionary<string, string> replacementDictionary) =>
                TryCatch(() =>
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

                    return template;
                });

        public void ValidateTransformation(string content) =>
            TryCatch(() =>
            {
                ValidateTransformationArguments(content);

                CheckAllTagsHasBeenReplaced(content);
            });

        public Template ConvertStringToTemplate(string content) =>
            TryCatch(() =>
            {
                ValidateConvertStringToTemplateArguments(content);

                Template template =
                    JsonConvert.DeserializeObject<Template>(content);

                template.RawTemplate = content;
                ValidateTemplate(template);

                return template;
            });

        public string AppendContent(
            string sourceContent,
            string doesNotContain,
            string regexToMatchForAppend,
            string appendContent,
            bool appendToBeginning,
            bool appendEvenIfContentAlreadyExist) =>
                TryCatch(() =>
                {
                    ValidateAppendContent(sourceContent, regexToMatchForAppend, appendContent);

                    if (!string.IsNullOrWhiteSpace(doesNotContain) && sourceContent.Contains(doesNotContain))
                    {
                        return sourceContent;
                    }

                    var (matchFound, match) =
                        this.regularExpressionBroker
                            .CheckForExpressionMatch(regexToMatchForAppend, sourceContent);

                    ValidateExpressionMatch(matchFound);

                    if (appendEvenIfContentAlreadyExist == false && match.Contains(appendContent))
                    {
                        return sourceContent;
                    }

                    string mergedContent = AppendContentToExistingContent(appendContent, appendToBeginning, match);

                    string result = this.regularExpressionBroker
                        .Replace(sourceContent, regexToMatchForAppend, mergedContent);

                    return result;
                });

        private static string AppendContentToExistingContent(string appendContent, bool appendToBeginning, string match)
        {
            StringBuilder builder = new StringBuilder();
            if (appendToBeginning)
            {
                builder.Append(appendContent);
                builder.Append(match);
            }
            else
            {
                builder.Append(match);
                builder.Append(appendContent);
            }

            return builder.ToString();
        }
    }
}
