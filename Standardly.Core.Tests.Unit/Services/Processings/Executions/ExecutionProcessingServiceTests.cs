// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Moq;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Services.Foundations.Executions;
using Standardly.Core.Services.Processings.Executions;
using Tynamix.ObjectFiller;

namespace Standardly.Core.Tests.Unit.Services.Processings.Executions
{
    public partial class ExecutionProcessingServiceTests
    {
        private readonly Mock<IExecutionService> executionServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IExecutionProcessingService executionProcessingService;

        public ExecutionProcessingServiceTests()
        {
            this.executionServiceMock = new Mock<IExecutionService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.executionProcessingService = new ExecutionProcessingService(
                executionService: this.executionServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
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
