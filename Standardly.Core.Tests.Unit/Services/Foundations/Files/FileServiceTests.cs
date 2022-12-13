// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Moq;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Services.Foundations.Files;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Files
{
    public partial class FileServiceTests
    {
        private readonly Mock<IFileBroker> fileBrokerMock;
        private readonly IRetryConfig retryConfig;
        private readonly IFileService fileService;

        public FileServiceTests()
        {
            this.fileBrokerMock = new Mock<IFileBroker>();
            int maxRetryAttempts = 3;
            TimeSpan pauseBetweenFailures = TimeSpan.FromMilliseconds(10);
            this.retryConfig = new RetryConfig(maxRetryAttempts, pauseBetweenFailures);

            this.fileService = new FileService(
                fileBroker: this.fileBrokerMock.Object,
                retryConfig: this.retryConfig);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData FileServiceDependencyValidationExceptions()
        {
            return new TheoryData<Exception>()
            {
                new ArgumentNullException(),
                new ArgumentOutOfRangeException(),
                new ArgumentException()
            };
        }

        public static TheoryData FileServiceDependencyExceptions()
        {
            return new TheoryData<Exception>()
            {
                new SerializationException(),
                new IOException(),
            };
        }

        public static TheoryData CriticalFileDependencyExceptions()
        {
            return new TheoryData<Exception>()
            {
                new OutOfMemoryException(),
                new UnauthorizedAccessException()
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static List<string> GetRandomStringList()
        {
            List<string> stringList = new List<string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                stringList.Add(GetRandomString());
            }

            return stringList;
        }
    }
}
