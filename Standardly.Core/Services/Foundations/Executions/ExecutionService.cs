﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Services.Foundations.Executions
{
    public partial class ExecutionService : IExecutionService
    {
        private readonly IExecutionBroker executionBroker;

        public ExecutionService(IExecutionBroker executionBroker)
        {
            this.executionBroker = executionBroker;
        }

        public string Run(List<Execution> executions, string executionFolder) =>
            TryCatch(() =>
            {
                ValidateRunArguments(executions, executionFolder);

                List<Execution> executionList = new List<Execution>
                {
                    new Execution("Execution Folder", $"cd /d \"{executionFolder}\"")
                };

                executionList.AddRange(executions);

                return this.executionBroker.Run(executions, executionFolder);
            });
    }
}
