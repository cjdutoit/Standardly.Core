// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Foundations.Files.Exceptions;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService : IFileService
    {
        private void ValidateCheckIfFileExistsArguments(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private void ValidateWriteToFileArguments(string path, string content)
        {
            Validate(
                (Rule: IsInvalid(path), Parameter: nameof(path)),
                (Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private void ValidateReadFromFileArguments(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private void ValidateDeleteFileArguments(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private void ValidateRetrieveListOfFilesArguments(string path, string searchPattern)
        {
            Validate(
                (Rule: IsInvalid(path), Parameter: nameof(path)),
                (Rule: IsInvalid(searchPattern), Parameter: nameof(searchPattern)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentFileException = new InvalidArgumentFileException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentFileException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentFileException.ThrowIfContainsErrors();
        }
    }
}
