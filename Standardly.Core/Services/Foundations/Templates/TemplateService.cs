// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;

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

        public ValueTask<string> TransformString(string content, Dictionary<string, string> replacementDictionary) =>
            throw new NotImplementedException();
    }
}
