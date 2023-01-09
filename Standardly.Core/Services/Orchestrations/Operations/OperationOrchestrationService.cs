// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
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
         TryCatch(async () =>
         {
             ValidateWriteToFile(path, content);
             FileInfo fileName = new FileInfo(path);
             string directoryPath = fileName.DirectoryName;

             bool directoryExists = await this.fileProcessingService.CheckIfDirectoryExistsAsync(directoryPath);

             if (!directoryExists)
             {
                 await this.fileProcessingService.CreateDirectoryAsync(directoryPath);
             }

             return await this.fileProcessingService.WriteToFileAsync(path, content);
         });

        public ValueTask<string> ReadFromFileAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateReadFromFile(path);

                return await this.fileProcessingService.ReadFromFileAsync(path);
            });

        public ValueTask<bool> DeleteFileAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateDeleteFile(path);

                return await this.fileProcessingService.DeleteFileAsync(path);
            });

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
