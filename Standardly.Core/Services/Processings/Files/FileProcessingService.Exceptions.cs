// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Files
{
    public partial class FileProcessingService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<bool> TryCatchAsync(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidFileProcessingException invalidPathFileProcessingException)
            {
                throw CreateAndLogValidationException(invalidPathFileProcessingException);
            }
            catch (FileValidationException fileValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileValidationException);
            }
            catch (FileDependencyValidationException fileDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(fileDependencyValidationException);
            }
            catch (FileDependencyException fileDependencyException)
            {
                throw CreateAndLogDependencyException(fileDependencyException);
            }
            catch (FileServiceException fileServiceException)
            {
                throw CreateAndLogDependencyException(fileServiceException);
            }
            catch (Exception exception)
            {
                var failedFileProcessingServiceException =
                    new FailedFileProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedFileProcessingServiceException);
            }
        }

        private async ValueTask TryCatchAsync(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidFileProcessingException invalidPathFileProcessingException)
            {
                throw CreateAndLogValidationException(invalidPathFileProcessingException);
            }
        }

        private FileProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var fileProcessingValidationException =
                new FileProcessingValidationException(exception);

            this.loggingBroker.LogError(fileProcessingValidationException);

            return fileProcessingValidationException;
        }

        private FileProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var fileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(fileProcessingDependencyValidationException);

            return fileProcessingDependencyValidationException;
        }

        private FileProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var fileProcessingDependencyException =
                new FileProcessingDependencyException(
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(fileProcessingDependencyException);

            return fileProcessingDependencyException;
        }

        private FileProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var fileProcessingServiceException = new
                FileProcessingServiceException(exception);

            this.loggingBroker.LogError(fileProcessingServiceException);

            return fileProcessingServiceException;
        }
    }
}
