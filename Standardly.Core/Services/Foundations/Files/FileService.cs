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

        public async ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateCheckIfFileExistsArguments(path);

                        return this.fileBroker.CheckIfFileExists(path);
                    });
                }));

        public async ValueTask<bool> WriteToFileAsync(string path, string content) =>
            await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateWriteToFileArguments(path, content);

                        return this.fileBroker.WriteToFile(path, content);
                    });
                }));

        public async ValueTask<string> ReadFromFileAsync(string path) =>
            await Task.Run(() =>
                TryCatch(() =>
                 {
                     return WithRetry(() =>
                     {
                         ValidateReadFromFileArguments(path);

                         return this.fileBroker.ReadFile(path);
                     });
                 }));

        public async ValueTask<bool> DeleteFileAsync(string path) =>
             await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateDeleteFileArguments(path);
                        return this.fileBroker.DeleteFile(path);
                    });
                }));

        public async ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateRetrieveListOfFilesArguments(path, searchPattern);
                        return this.fileBroker.GetListOfFiles(path, searchPattern);
                    });
                }));

        public async ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            await Task.Run(() =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateCheckIfDirectoryExistsArguments(path);

                    return this.fileBroker.CheckIfDirectoryExists(path);
                });
            }));

        public async ValueTask<bool> CreateDirectoryAsync(string path) =>
            await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateCreateDirectoryArguments(path);

                        return this.fileBroker.CreateDirectory(path);
                    });
                }));

        public async ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            await Task.Run(() =>
                TryCatch(() =>
                {
                    return WithRetry(() =>
                    {
                        ValidateDeleteDirectoryArguments(path);

                        return this.fileBroker.DeleteDirectory(path, recursive);
                    });
                }));
    }
}
