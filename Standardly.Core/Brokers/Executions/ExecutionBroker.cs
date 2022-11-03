// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Brokers.Executions
{
    public class ExecutionBroker : IExecutionBroker
    {
        public async ValueTask<string> RunAsync(List<Execution> executions, string executionFolder)
        {
            List<Execution> executionList = new List<Execution>
            {
                new Execution("Execution Folder", $"cd /d \"{executionFolder}\"")
            };

            executionList.AddRange(executions);

            StringBuilder outputMessages = new StringBuilder();

            using (CmdService cmdService =
                new CmdService(
                    "cmd.exe",
                    "/k \"C:\\Program Files\\Microsoft Visual Studio\\2022\\Enterprise\\Common7\\Tools\\VsDevCmd.bat\""))
            {
                foreach (Execution execution in executionList)
                {
                    string output = cmdService.ExecuteCommand(execution.Instruction);
                    outputMessages.AppendLine(output);
                }
            }

            return await Task.FromResult(outputMessages.ToString());
        }
    }
}
