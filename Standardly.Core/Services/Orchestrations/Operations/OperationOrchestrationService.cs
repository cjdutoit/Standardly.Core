// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;

namespace Standardly.Core.Services.Orchestrations.Operations
{
    public partial class OperationOrchestrationService
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
    }
}
