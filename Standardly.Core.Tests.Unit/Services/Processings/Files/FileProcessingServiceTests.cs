﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Moq;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Services.Foundations.Files;
using Standardly.Core.Services.Processings.Files;
using Tynamix.ObjectFiller;

namespace Standardly.Core.Tests.Unit.Services.Processings.Files
{
    public partial class FileProcessingServiceTests
    {
        private readonly Mock<IFileService> fileServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IFileProcessingService fileProcessingService;

        public FileProcessingServiceTests()
        {
            this.fileServiceMock = new Mock<IFileService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.fileProcessingService = new FileProcessingService(
                fileService: this.fileServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
