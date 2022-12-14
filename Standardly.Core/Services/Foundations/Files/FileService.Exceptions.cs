// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Files.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask ReturningNothingFunction();

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
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentNullException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentOutOfRangeException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(serializationException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(outOfMemoryException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(ioException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw CreateAndLogValidationException(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentNullException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentOutOfRangeException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(serializationException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(ioException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(outOfMemoryException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw CreateAndLogValidationException(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentNullException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentOutOfRangeException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(serializationException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(outOfMemoryException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(ioException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentFileException invalidArgumentFileException)
            {
                throw CreateAndLogValidationException(invalidArgumentFileException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentNullException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentOutOfRangeException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (ArgumentException argumentException)
            {
                var invalidFileDependencyException =
                    new InvalidFileServiceDependencyException(argumentException);

                throw CreateAndLogDependencyValidationException(invalidFileDependencyException);
            }
            catch (SerializationException serializationException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(serializationException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(outOfMemoryException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (IOException ioException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(ioException);

                throw CreateAndLogDependencyException(failedFileDependencyException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                var failedFileDependencyException =
                    new FailedFileDependencyException(unauthorizedAccessException);

                throw CreateAndLogCriticalDependencyException(failedFileDependencyException);
            }
            catch (Exception exception)
            {
                var failedFileServiceException =
                    new FailedFileServiceException(exception);

                throw CreateAndLogServiceException(failedFileServiceException);
            }
        }

        private FileValidationException CreateAndLogValidationException(Xeption exception)
        {
            var fileValidationException = new FileValidationException(exception);

            return fileValidationException;
        }

        private FileDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var fileServiceDependencyValidationException =
                new FileDependencyValidationException(exception);

            return fileServiceDependencyValidationException;
        }

        private FileDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var fileServiceDependencyException = new FileDependencyException(exception);

            return fileServiceDependencyException;
        }

        private FileDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var fileServiceDependencyException = new FileDependencyException(exception);

            return fileServiceDependencyException;
        }

        private FileServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var fileServiceException = new FileServiceException(exception);

            return fileServiceException;
        }
    }
}
