// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Models.Configurations.Retries;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService : IFileService
    {
        private readonly IFileBroker fileBroker;
        private readonly IRetryConfig retryConfig;
        private readonly ILoggingBroker loggingBroker;

        public FileService(IFileBroker fileBroker, IRetryConfig retryConfig, ILoggingBroker loggingBroker)
        {
            this.fileBroker = fileBroker;
            this.retryConfig = retryConfig;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCheckIfFileExistsArguments(path);

                return await this.fileBroker.CheckIfFileExistsAsync(path);
            });

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            TryCatchAsync(async () =>
            {
                ValidateWriteToFileArguments(path, content);

                return await this.fileBroker.WriteToFileAsync(path, content);
            });

        public ValueTask<string> ReadFromFileAsync(string path) =>
             TryCatchAsync(async () =>
             {
                 ValidateReadFromFileArguments(path);

                 return await this.fileBroker.ReadFileAsync(path);
             });

        public ValueTask<bool> DeleteFileAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateDeleteFileArguments(path);

                return await this.fileBroker.DeleteFileAsync(path);
            });

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            TryCatchAsync(async () =>
            {
                ValidateRetrieveListOfFilesArguments(path, searchPattern);

                return await this.fileBroker.GetListOfFilesAsync(path, searchPattern);
            });

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCheckIfDirectoryExistsArguments(path);

                return await this.fileBroker.CheckIfDirectoryExistsAsync(path);
            });

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            TryCatchAsync(async () =>
            {
                ValidateCreateDirectoryArguments(path);

                return await this.fileBroker.CreateDirectoryAsync(path);
            });

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            TryCatchAsync(async () =>
            {
                ValidateDeleteDirectoryArguments(path);

                return await this.fileBroker.DeleteDirectoryAsync(path, recursive);
            });
    }
}
