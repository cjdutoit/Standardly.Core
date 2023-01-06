// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Moq;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Services.Orchestrations.Operations;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Tynamix.ObjectFiller;

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
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

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
