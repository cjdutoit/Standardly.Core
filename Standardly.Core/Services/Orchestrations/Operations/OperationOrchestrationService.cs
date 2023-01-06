// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;

namespace Standardly.Core.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationService : IOperationOrchestrationService
    {
        private readonly IExecutionProcessingService executionProcessingService;
        private readonly IFileProcessingService fileProcessingService;

        public OperationOrchestrationService(
            IExecutionProcessingService executionProcessingService,
            IFileProcessingService fileProcessingService)
        {
            this.executionProcessingService = executionProcessingService;
            this.fileProcessingService = fileProcessingService;
        }

        public async ValueTask<string> RunAsync(List<Execution> executions, string executionFolder) =>
            await this.executionProcessingService.RunAsync(executions, executionFolder);
    }
}
