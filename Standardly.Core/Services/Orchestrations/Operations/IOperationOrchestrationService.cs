// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Services.Orchestrations.Operations
{
    public interface IOperationOrchestrationService
    {
        ValueTask<string> RunAsync(List<Execution> executions, string executionFolder);
    }
}
