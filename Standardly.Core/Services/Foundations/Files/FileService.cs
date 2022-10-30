// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
            TryCatch(async () =>
            {
                ValidateCheckIfFileExistsArguments(path);

                return await new ValueTask<bool>(this.fileBroker.CheckIfFileExists(path));
            });

        public ValueTask WriteToFileAsync(string path, string content) =>
            TryCatch(async () =>
            {
                ValidateWriteToFileArguments(path, content);
                await Task.Run(() => this.fileBroker.WriteToFile(path, content));
            });

        public ValueTask<string> ReadFromFileAsync(string path) =>
             TryCatch(async () =>
             {
                 ValidateReadFromFileArguments(path);

                 return await new ValueTask<string>(this.fileBroker.ReadFile(path));
             });

        public ValueTask DeleteFileAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateDeleteFileArguments(path);
                await Task.Run(() => this.fileBroker.DeleteFile(path));
            });

        public ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*") =>
            TryCatch(async () =>
            {
                ValidateRetrieveListOfFilesArguments(path, searchPattern);

                return await new ValueTask<List<string>>(this.fileBroker.GetListOfFiles(path, searchPattern).ToList());
            });

        public ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCheckIfDirectoryExistsArguments(path);

                return await new ValueTask<bool>(this.fileBroker.CheckIfDirectoryExists(path));
            });

        public ValueTask CreateDirectoryAsync(string path) =>
            TryCatch(async () =>
            {
                ValidateCreateDirectoryArguments(path);
                await Task.Run(() => this.fileBroker.CreateDirectory(path));
            });

        public async ValueTask DeleteDirectoryAsync(string path, bool recursive = false) =>
            await Task.Run(() => this.fileBroker.DeleteDirectory(path, recursive));
    }
}
