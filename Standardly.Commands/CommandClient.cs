// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Standardly.Commands
{
    public class CommandClient : IDisposable
    {
        private readonly Process cmdProcess;
        private readonly StreamWriter streamWriter;
        private readonly AutoResetEvent outputWaitHandle;
        private string cmdOutput;

        /// <summary>
        /// A command client that to run commands. 
        /// </summary>
        /// <param name="commandPath">
        ///     Summary:
        ///        Gets or sets the application or document to start.
        ///
        ///     Returns:
        ///        The name of the application to start, or the name of a document of a file type
        ///        that is associated with an application and that has a default open action available
        ///        to it. The default is an empty string ("").
        /// </param>
        /// <param name="useShellExecute">
        ///     Summary:
        ///         Gets or sets a value indicating whether to use the operating system shell to
        ///         start the process.
        ///
        ///     Returns:
        ///         true if the shell should be used when starting the process; false if the process
        ///         should be created directly from the executable file. The default is true.
        /// </param>
        /// <param name="createNoWindow">
        ///     Summary:
        ///         Gets or sets a value indicating whether to start the process in a new window.
        ///
        ///     Returns:
        ///         true if the process should be started without creating a new window to contain
        ///         it; otherwise, false. The default is false.
        /// </param>
        public CommandClient(string commandPath, string arguments = "", bool useShellExecute = false, bool createNoWindow = false)
        {
            cmdProcess = new Process();
            outputWaitHandle = new AutoResetEvent(false);
            cmdOutput = String.Empty;

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = commandPath,
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = createNoWindow
            };

            if (!string.IsNullOrWhiteSpace(arguments))
            {
                processStartInfo.Arguments = arguments;
            }

            cmdProcess.OutputDataReceived += cmdProcessOutputDataReceived;

            cmdProcess.StartInfo = processStartInfo;
            cmdProcess.Start();

            streamWriter = cmdProcess.StandardInput;
            cmdProcess.BeginOutputReadLine();
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command">The application to run or document to open.</param>
        /// <returns>Returns a string output for the action taken.</returns>
        public string ExecuteCommand(string command)
        {
            cmdOutput = String.Empty;

            streamWriter.WriteLine(command);
            streamWriter.WriteLine(" echo end");
            outputWaitHandle.WaitOne();
            return cmdOutput;
        }

        /// <summary>
        /// Executes multiple commands.
        /// </summary>
        /// <param name="commands">The command list of application to run or documents to open.</param>
        /// <returns>Returns a string output for the action taken.</returns>
        public string ExecuteCommand(List<string> commands)
        {
            StringBuilder commandOutput = new StringBuilder();

            foreach (string command in commands)
            {
                commandOutput.AppendLine(this.ExecuteCommand(command));
            }

            return commandOutput.ToString();
        }


        private void cmdProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null || e.Data == "end")
                outputWaitHandle.Set();
            else
                cmdOutput += e.Data + Environment.NewLine;
        }

        public void Dispose()
        {
            cmdProcess.Close();
            cmdProcess.Dispose();
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}
