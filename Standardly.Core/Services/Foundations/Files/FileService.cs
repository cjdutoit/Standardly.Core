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

        public async ValueTask<bool> CheckIfFileExists(string path) =>
            await new ValueTask<bool>(this.fileBroker.CheckIfFileExists(path));

        public ValueTask WriteToFile(string path, string content) =>
            throw new System.NotImplementedException();

        public ValueTask<string> ReadFromFile(string path) =>
            throw new System.NotImplementedException();

        public ValueTask DeleteFile(string path) =>
            throw new System.NotImplementedException();

        public ValueTask<List<string>> RetrieveListOfFiles(string path, string searchPattern = "*") =>
            throw new System.NotImplementedException();

        public ValueTask<bool> CheckIfDirectoryExists(string path) =>
            throw new System.NotImplementedException();

        public ValueTask CreateDirectory(string path) =>
            throw new System.NotImplementedException();

        public ValueTask DeleteDirectory(string path, bool recursive = false) =>
            throw new System.NotImplementedException();
    }
}
