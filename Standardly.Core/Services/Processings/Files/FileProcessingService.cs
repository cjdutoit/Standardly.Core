// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
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

        public bool CheckIfFileExists(string path) =>
            TryCatch(() =>
            {
                ValidateCheckIfFileExists(path);

                return this.fileService.CheckIfFileExists(path);
            });

        public bool WriteToFile(string path, string content) =>
            TryCatch(() =>
            {
                ValidateWriteToFile(path, content);
                FileInfo fileName = new FileInfo(path);
                string directoryPath = fileName.DirectoryName;

                if (!this.fileService.CheckIfDirectoryExists(directoryPath))
                {
                    this.fileService.CreateDirectory(directoryPath);
                }

                return this.fileService.WriteToFile(path, content);
            });

        public string ReadFromFile(string path) =>
            TryCatch(() =>
            {
                ValidateReadFromFile(path);

                return this.fileService.ReadFromFile(path);
            });

        public bool DeleteFile(string path) =>
             TryCatch(() =>
             {
                 ValidateDeleteFile(path);

                 return this.fileService.DeleteFile(path);
             });

        public List<string> RetrieveListOfFiles(string path, string searchPattern = "*") =>
            TryCatch(() =>
            {
                ValidateRetrieveListOfFiles(path, searchPattern);

                return this.fileService.RetrieveListOfFiles(path, searchPattern);
            });

        public bool CheckIfDirectoryExists(string path) =>
            TryCatch(() =>
            {
                ValidateCheckIfDirectoryExists(path);

                return this.fileService.CheckIfDirectoryExists(path);
            });

        public bool CreateDirectory(string path) =>
            TryCatch(() =>
            {
                ValidateCreateDirectory(path);
                return this.fileService.CreateDirectory(path);
            });

        public bool DeleteDirectory(string path, bool recursive = false) =>
            TryCatch(() =>
            {
                ValidateDeleteDirectory(path);
                return this.fileService.DeleteDirectory(path, recursive);
            });
    }
}
