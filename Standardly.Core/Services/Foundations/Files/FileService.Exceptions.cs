// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService : IFileService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw CreateAndLogValidationException(invalidArgumentFileException);
            }
        }

        private FileValidationException CreateAndLogValidationException(Xeption exception)
        {
            var fileValidationException = new FileValidationException(exception);
            this.loggingBroker.LogError(fileValidationException);

            return fileValidationException;
        }
    }
}
