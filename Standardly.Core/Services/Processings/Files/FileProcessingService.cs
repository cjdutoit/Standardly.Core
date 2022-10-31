// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

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

        public ValueTask WriteToFileAsync(string path, string content) =>
            throw new System.NotImplementedException();
    }
}
