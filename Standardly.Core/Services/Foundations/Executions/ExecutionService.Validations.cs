// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Executions.Exceptions;

namespace Standardly.Core.Services.Foundations.Executions
{
    public partial class ExecutionService
    {
        private void ValidateRunArguments(List<Execution> executions, string executionFolder)
        {
            Validate(
                (Rule: IsInvalid(executionFolder), Parameter: nameof(executionFolder)),
                (Rule: IsInvalid(executions), Parameter: nameof(executions)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(List<Execution> executions) => new
        {
            Condition = ValidateIfExecutionsIsInvalid(executions),
            Message = "Executions is required"
        };

        private static bool ValidateIfExecutionsIsInvalid(List<Execution> executions)
        {
            if (executions == null)
            {
                return true;
            }


            foreach (Execution execution in executions)
            {
                if (
                    execution == null
                    || String.IsNullOrWhiteSpace(execution.Name)
                    || String.IsNullOrWhiteSpace(execution.Instruction))
                {
                    return true;
                }
            };

            return false;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentExecutionException = new InvalidArgumentExecutionException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentExecutionException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentExecutionException.ThrowIfContainsErrors();
        }
    }
}
