// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Standardly.Core.Services.Foundations.Files;

namespace Standardly.Core.Services.Processings.Files
{
    public partial class FileProcessingService : IFileProcessingService
    {
        private readonly IFileService fileService;

        public FileProcessingService(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCheckIfFileExists(path);

                return await this.fileService.CheckIfFileExistsAsync(path);
            });

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            TryCatch(async () =>
            {
                ValidateWriteToFile(path, content);
                FileInfo fileName = new FileInfo(path);
                string directoryPath = fileName.DirectoryName;

                bool directoryExists = await this.fileService.CheckIfDirectoryExistsAsync(directoryPath);

                if (!directoryExists)
                {
                    await this.fileService.CreateDirectoryAsync(directoryPath);
                }

                return await this.fileService.WriteToFileAsync(path, content);
            });

        public ValueTask<string> ReadFromFileAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateReadFromFile(path);

                return await this.fileService.ReadFromFileAsync(path);
            });

        public ValueTask<bool> DeleteFileAsync(string path) =>
             TryCatch(async () =>
             {
                 ValidateDeleteFile(path);

                 return await this.fileService.DeleteFileAsync(path);
             });

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            TryCatch(async () =>
            {
                ValidateRetrieveListOfFiles(path, searchPattern);

                return await this.fileService.RetrieveListOfFilesAsync(path, searchPattern);
            });

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCheckIfDirectoryExists(path);

                return await this.fileService.CheckIfDirectoryExistsAsync(path);
            });

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCreateDirectory(path);
                return await this.fileService.CreateDirectoryAsync(path);
            });

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            TryCatch(async () =>
            {
                ValidateDeleteDirectory(path);
                return await this.fileService.DeleteDirectoryAsync(path, recursive);
            });
    }
}
