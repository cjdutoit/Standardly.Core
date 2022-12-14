// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Moq;
using Standardly.Core.Models.Services.Foundations.Executions;
using Standardly.Core.Models.Services.Processings.Executions.Exceptions;
using Standardly.Core.Models.Services.Processings.Files.Exceptions;
using Standardly.Core.Services.Orchestrations.Operations;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationServiceTests
    {
        private readonly Mock<IFileProcessingService> fileProcessingServiceMock;
        private readonly Mock<IExecutionProcessingService> executionProcessingServiceMock;
        private readonly IOperationOrchestrationService operationOrchestrationService;

        public OperationOrchestrationServiceTests()
        {
            this.fileProcessingServiceMock = new Mock<IFileProcessingService>();
            this.executionProcessingServiceMock = new Mock<IExecutionProcessingService>();

            this.operationOrchestrationService = new OperationOrchestrationService(
                executionProcessingService: this.executionProcessingServiceMock.Object,
                fileProcessingService: this.fileProcessingServiceMock.Object);
        }

        public static TheoryData ExecutionDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ExecutionProcessingValidationException(innerException),
                new ExecutionProcessingDependencyValidationException(innerException)
            };
        }

        public static TheoryData ExecutionDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ExecutionProcessingDependencyException(innerException),
                new ExecutionProcessingServiceException(innerException)
            };
        }

        public static TheoryData FileDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new FileProcessingValidationException(innerException),
                new FileProcessingDependencyValidationException(innerException)
            };
        }

        public static TheoryData FileDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new FileProcessingDependencyException(innerException),
                new FileProcessingServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static List<string> GetRandomStringList()
        {
            List<string> stringList = new List<string>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                stringList.Add(GetRandomString());
            }

            return stringList;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static List<Execution> GetRandomExecutions()
        {
            List<Execution> executions = new List<Execution>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                executions.Add(new Execution(name: GetRandomString(), instruction: GetRandomString()));
            }

            return executions;
        }
    }
}
