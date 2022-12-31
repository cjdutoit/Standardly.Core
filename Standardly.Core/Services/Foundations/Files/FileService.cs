// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Models.Configurations.Retries;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService : IFileService
    {
        private readonly IFileBroker fileBroker;
        private readonly IRetryConfig retryConfig;

        public FileService(IFileBroker fileBroker, IRetryConfig retryConfig)
        {
            this.fileBroker = fileBroker;
            this.retryConfig = retryConfig;
        }

        public ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateCheckIfFileExistsArguments(path);

                    return await this.fileBroker.CheckIfFileExistsAsync(path);
                });
            });

        public ValueTask<bool> WriteToFileAsync(string path, string content) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateWriteToFileArguments(path, content);

                    return await this.fileBroker.WriteToFileAsync(path, content);
                });
            });

        public ValueTask<string> ReadFromFileAsync(string path) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateReadFromFileArguments(path);

                    return await this.fileBroker.ReadFileAsync(path);
                });
            });

        public ValueTask<bool> DeleteFileAsync(string path) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateDeleteFileArguments(path);
                    return await this.fileBroker.DeleteFileAsync(path);
                });
            });

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateRetrieveListOfFilesArguments(path, searchPattern);
                    return await this.fileBroker.GetListOfFilesAsync(path, searchPattern);
                });
            });

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateCheckIfDirectoryExistsArguments(path);

                    return await this.fileBroker.CheckIfDirectoryExistsAsync(path);
                });
            });

        public ValueTask<bool> CreateDirectoryAsync(string path) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateCreateDirectoryArguments(path);

                    return await this.fileBroker.CreateDirectoryAsync(path);
                });
            });

        public ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateDeleteDirectoryArguments(path);

                    return await this.fileBroker.DeleteDirectoryAsync(path, recursive);
                });
            });
    }
}
