// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Services.Foundations.Templates;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Templates
{
    public partial class TemplateServiceTests
    {
        private readonly IFileBroker fileBroker;
        private readonly IRegularExpressionBroker regularExpressionBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITemplateService templateService;

        public TemplateServiceTests()
        {
            this.fileBroker = new FileBroker();
            this.regularExpressionBroker = new RegularExpressionBroker();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.templateService = new TemplateService(
                fileBroker: fileBroker,
                regularExpressionBroker: regularExpressionBroker,
                loggingBroker: loggingBrokerMock.Object);
        }
    }
}
