// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Processings.Templates.Exceptions;

namespace Standardly.Core.Services.Processings.Executions
{
    public partial class ExecutionProcessingService
    {
        private void ValidateRunArguments(List<Execution> executions, string executionFolder)
        {
            Validate(
                (Rule: IsInvalid(executions), Parameter: nameof(executions)),
                (Rule: IsInvalid(executionFolder), Parameter: nameof(executionFolder)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(List<Execution> executions) => new
        {
            Condition = executions == null,
            Message = "Executions is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentExecutionProcessingException = new InvalidArgumentExecutionProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentExecutionProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentExecutionProcessingException.ThrowIfContainsErrors();
        }
    }
}
