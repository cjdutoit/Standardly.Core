﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Services.Foundations.Files;

namespace Standardly.Core.Services.Processings.Files
{
    public partial class FileProcessingService : IFileProcessingService
    {
        private readonly IFileService fileService;
        private readonly ILoggingBroker loggingBroker;

        public FileProcessingService(IFileService fileService, ILoggingBroker loggingBroker)
        {
            this.fileService = fileService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCheckIfFileExists(path);

                return await this.fileService.CheckIfFileExistsAsync(path);
            });

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            TryCatchAsync(async () =>
            {
                ValidateWriteToFile(path, content);
                FileInfo fileName = new FileInfo(path);
                string directoryPath = fileName.DirectoryName;

                if (!this.fileService.CheckIfDirectoryExistsAsync(directoryPath).Result)
                {
                    await this.fileService.CreateDirectoryAsync(directoryPath);
                }

                return await this.fileService.WriteToFileAsync(path, content);
            });

        public ValueTask<string> ReadFromFileAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateReadFromFile(path);

                return await this.fileService.ReadFromFileAsync(path);
            });

        public ValueTask<bool> DeleteFileAsync(string path) =>
             TryCatchAsync(async () =>
             {
                 ValidateDeleteFile(path);

                 return await this.fileService.DeleteFileAsync(path);
             });

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            TryCatchAsync(async () =>
            {
                ValidateRetrieveListOfFiles(path, searchPattern);

                return await this.fileService.RetrieveListOfFilesAsync(path, searchPattern);
            });

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCheckIfDirectoryExists(path);

                return await this.fileService.CheckIfDirectoryExistsAsync(path);
            });

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCreateDirectory(path);
                return await this.fileService.CreateDirectoryAsync(path);
            });

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            TryCatchAsync(async () =>
            {
                ValidateDeleteDirectory(path);
                return await this.fileService.DeleteDirectoryAsync(path, recursive);
            });
    }
}
