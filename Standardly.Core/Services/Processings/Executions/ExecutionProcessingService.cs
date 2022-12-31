// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Services.Foundations.Executions;

namespace Standardly.Core.Services.Processings.Executions
{
    public partial class ExecutionProcessingService : IExecutionProcessingService
    {
        private readonly IExecutionService executionService;

        public ExecutionProcessingService(IExecutionService executionService)
        {
            this.executionService = executionService;
        }

        public ValueTask<string> RunAsync(List<Execution> executions, string executionFolder) =>
            TryCatch(async () =>
            {
                ValidateRunArguments(executions, executionFolder);

                return await this.executionService.RunAsync(executions, executionFolder);
            });
    }
}
