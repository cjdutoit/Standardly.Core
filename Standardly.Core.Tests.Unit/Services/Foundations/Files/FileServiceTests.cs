// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Moq;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Services.Foundations.Files;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        private readonly Mock<IFileBroker> fileBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IRetryConfig retryConfig;
        private readonly IFileService fileService;

        public FileServiceTests()
        {
            this.fileBrokerMock = new Mock<IFileBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            int maxRetryAttempts = 3;
            TimeSpan pauseBetweenFailures = TimeSpan.FromMilliseconds(10);
            this.retryConfig = new RetryConfig(maxRetryAttempts, pauseBetweenFailures);

            this.fileService = new FileService(
                fileBroker: this.fileBrokerMock.Object,
                retryConfig: this.retryConfig,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
