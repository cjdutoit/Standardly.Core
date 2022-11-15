// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
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

        public bool CheckIfFileExists(string path) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateCheckIfFileExistsArguments(path);

                    return this.fileBroker.CheckIfFileExists(path);
                });
            });

        public bool WriteToFile(string path, string content) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateWriteToFileArguments(path, content);

                    return this.fileBroker.WriteToFile(path, content);
                });
            });

        public string ReadFromFile(string path) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateReadFromFileArguments(path);

                    return this.fileBroker.ReadFile(path);
                });
            });

        public bool DeleteFile(string path) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateDeleteFileArguments(path);
                    return this.fileBroker.DeleteFile(path);
                });
            });

        public List<string> RetrieveListOfFiles(string path, string searchPattern = "*") =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateRetrieveListOfFilesArguments(path, searchPattern);
                    return this.fileBroker.GetListOfFiles(path, searchPattern);
                });
            });

        public bool CheckIfDirectoryExists(string path) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateCheckIfDirectoryExistsArguments(path);

                    return this.fileBroker.CheckIfDirectoryExists(path);
                });
            });

        public bool CreateDirectory(string path) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateCreateDirectoryArguments(path);

                    return this.fileBroker.CreateDirectory(path);
                });
            });

        public bool DeleteDirectory(string path, bool recursive = false) =>
            TryCatch(() =>
            {
                return WithRetry(() =>
                {
                    ValidateDeleteDirectoryArguments(path);

                    return this.fileBroker.DeleteDirectory(path, recursive);
                });
            });
    }
}
