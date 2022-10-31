// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Services.Foundations.Executions
{
    public partial class ExecutionService : IExecutionService
    {
        private readonly IExecutionBroker executionBroker;
        private readonly ILoggingBroker loggingBroker;

        public ExecutionService(IExecutionBroker executionBroker, ILoggingBroker loggingBroker)
        {
            this.executionBroker = executionBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> RunAsync(List<Execution> executions, string executionFolder) =>
            TryCatchAsync(async () =>
            {
                ValidateRunArguments(executions, executionFolder);

                return await new ValueTask<string>(this.executionBroker.Run(executions, executionFolder));
            });
    }
}
