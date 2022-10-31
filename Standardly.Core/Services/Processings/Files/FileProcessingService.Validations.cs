﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Standardly.Core.Models.Processings.Files.Exceptions;

namespace Standardly.Core.Services.Processings.Files
{
    public partial class FileProcessingService
    {
        private static void ValidateCheckIfFileExists(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private static void ValidateWriteToFile(string path, string content)
        {
            Validate(
                (Rule: IsInvalid(path), Parameter: nameof(path)),
                (Rule: IsInvalid(content), Parameter: nameof(content)));
        }

        private static void ValidateReadFromFile(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private static void ValidateDeleteFile(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private static void ValidateRetrieveListOfFiles(string path, string searchPattern)
        {
            Validate(
                (Rule: IsInvalid(path), Parameter: nameof(path)),
                (Rule: IsInvalid(searchPattern), Parameter: nameof(searchPattern)));
        }

        private static void ValidateCheckIfDirectoryExists(string path)
        {
            Validate((Rule: IsInvalid(path), Parameter: nameof(path)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidFileProcessingException =
                new InvalidFileProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidFileProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidFileProcessingException.ThrowIfContainsErrors();
        }
    }
}
