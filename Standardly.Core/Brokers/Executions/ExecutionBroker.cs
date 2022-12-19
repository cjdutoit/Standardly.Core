// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standardly.Commands;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Brokers.Executions
{
    public class ExecutionBroker : IExecutionBroker
    {
        public async ValueTask<string> RunAsync(List<Execution> executions, string executionFolder)
        {
            return await Task.Run(() =>
            {
                using (CommandClient commandClient = new CommandClient("cmd.exe"))
                {
                    List<string> instructions = executions
                        .Select(execution => execution.Instruction).ToList();

                    return commandClient.ExecuteCommand(instructions);
                }
            });
        }
    }
}
