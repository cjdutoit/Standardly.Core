// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Services.Foundations.Executions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Foundations.Executions
{
    public partial class ExecutionServiceTests
    {
        private readonly Mock<IExecutionBroker> executionBrokerMock;
        private readonly IExecutionService executionService;

        public ExecutionServiceTests()
        {
            this.executionBrokerMock = new Mock<IExecutionBroker>();

            this.executionService = new ExecutionService(
                executionBroker: this.executionBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData InvalidExecutions()
        {
            return new TheoryData<List<Execution>>
            {
                null,
                new List<Execution>() { new Execution(name: null, instruction: GetRandomString()) },
                new List<Execution>() { new Execution(name: "", instruction: GetRandomString()) },
                new List<Execution>() { new Execution(name: "   ", instruction: GetRandomString()) },
                new List<Execution>() { new Execution(name: GetRandomString(), instruction: null) },
                new List<Execution>() { new Execution(name: GetRandomString(),   instruction: "") },
                new List<Execution>() { new Execution(name: GetRandomString(), instruction: "   ") },
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 5).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

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
