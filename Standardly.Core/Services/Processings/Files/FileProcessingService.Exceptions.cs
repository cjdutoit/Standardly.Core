// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Files.Exceptions;
using Standardly.Core.Models.Processings.Files.Exceptions;
using Xeptions;

namespace Standardly.Core.Services.Processings.Files
{
    public partial class FileProcessingService
    {
        private delegate bool ReturningBooleanFunction();
        private delegate string ReturningStringFunction();
        private delegate List<string> ReturningStringListFunction();
        private delegate void ReturningNothingFunction();

        private bool TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return returningBooleanFunction();
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

        private string TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return returningStringFunction();
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

        private List<string> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return returningStringListFunction();
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

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
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

        private FileProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var fileProcessingValidationException =
                new FileProcessingValidationException(exception);

            return fileProcessingValidationException;
        }

        private FileProcessingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var fileProcessingDependencyValidationException =
                new FileProcessingDependencyValidationException(
                    exception.InnerException as Xeption);

            return fileProcessingDependencyValidationException;
        }

        private FileProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var fileProcessingDependencyException =
                new FileProcessingDependencyException(
                    exception.InnerException as Xeption);

            return fileProcessingDependencyException;
        }

        private FileProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var fileProcessingServiceException = new
                FileProcessingServiceException(exception);

            return fileProcessingServiceException;
        }
    }
}
