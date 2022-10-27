﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Standardly.Core.Brokers.Executions
{
    public class CmdService : IDisposable
    {
        private readonly Process _cmdProcess;
        private readonly StreamWriter _streamWriter;
        private readonly AutoResetEvent _outputWaitHandle;
        private string _cmdOutput;

        public CmdService(string cmdPath, string arguments = "")
        {
            _cmdProcess = new Process();
            _outputWaitHandle = new AutoResetEvent(false);
            _cmdOutput = String.Empty;

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = cmdPath
            };

            if (!string.IsNullOrWhiteSpace(arguments))
            {
                //processStartInfo.Arguments = arguments;
            }

            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.CreateNoWindow = false;

            _cmdProcess.OutputDataReceived += _cmdProcess_OutputDataReceived;

            _cmdProcess.StartInfo = processStartInfo;
            _cmdProcess.Start();

            _streamWriter = _cmdProcess.StandardInput;
            _cmdProcess.BeginOutputReadLine();
        }

        public string ExecuteCommand(string command)
        {
            _cmdOutput = String.Empty;

            _streamWriter.WriteLine(command);
            _streamWriter.WriteLine(" echo end");
            _outputWaitHandle.WaitOne();
            return _cmdOutput;
        }

        private void _cmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null || e.Data == "end")
                _outputWaitHandle.Set();
            else
                _cmdOutput += e.Data + Environment.NewLine;
        }

        public void Dispose()
        {
            _cmdProcess.Close();
            _cmdProcess.Dispose();
            _streamWriter.Close();
            _streamWriter.Dispose();
        }
    }
}
