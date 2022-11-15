// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Standardly.Commands;
using System.Text;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Brokers.Executions
{
    public class ExecutionBroker : IExecutionBroker
    {
        public string Run(List<Execution> executions, string executionFolder)
        {
            using (CommandClient commandClient = new CommandClient("cmd.exe"))
            {
                List<string> instructions = executions
                    .Select(execution => execution.Instruction).ToList();

                return commandClient.ExecuteCommand(instructions);
            }
        }
    }
}
