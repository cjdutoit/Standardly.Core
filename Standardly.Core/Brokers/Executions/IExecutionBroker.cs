// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Standardly.Core.Models.Services.Foundations.Executions;

namespace Standardly.Core.Brokers.Executions
{
    public interface IExecutionBroker
    {
        ValueTask<string> RunAsync(List<Execution> executions, string executionFolder);
    }
}
