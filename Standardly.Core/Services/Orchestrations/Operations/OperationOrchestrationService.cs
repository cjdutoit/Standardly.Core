// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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

        public ValueTask<string> RunAsync(List<Execution> executions, string executionFolder) =>
            TryCatch(async () =>
            {
                ValidateRunArguments(executions, executionFolder);

                return await this.executionProcessingService.RunAsync(executions, executionFolder);
            });

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCheckIfFileExists(path);

                return await this.fileProcessingService.CheckIfFileExistsAsync(path);
            });

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            throw new NotImplementedException();

        public ValueTask<string> ReadFromFileAsync(string path) =>
            throw new NotImplementedException();

        public ValueTask<bool> DeleteFileAsync(string path) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            throw new NotImplementedException();

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            throw new NotImplementedException();

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            throw new NotImplementedException();

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            throw new NotImplementedException();
    }
}
