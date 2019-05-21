using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace Walterlv.Demo.InstallViaMounting.Utils
{
    public class CommandRunner
    {
        public string ExecutablePath { get; }
        public string WorkingDirectory { get; }

        public CommandRunner(string executablePath, string? workingDirectory = null)
        {
            ExecutablePath = executablePath ?? throw new ArgumentNullException(nameof(executablePath));
            WorkingDirectory = workingDirectory ?? Path.GetDirectoryName(executablePath);
        }

        public string Run(string arguments)
        {
            var info = new ProcessStartInfo(ExecutablePath, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory,
            };
            var process = new Process
            {
                StartInfo = info,
            };
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        public ICommandStream Start(string arguments)
        {
            var info = new ProcessStartInfo(ExecutablePath, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory,
            };
            var process = new Process
            {
                StartInfo = info,
            };
            process.Start();
            var stream = new CommandStream(process);
            return stream;
        }

        private class CommandStream : ICommandStream
        {
            private readonly Process _process;
            private readonly StreamWriter _input;
            private readonly StreamReader _output;

            public CommandStream(Process process)
            {
                _process = process;
                _input = process.StandardInput;
                _output = process.StandardOutput;
                process.OutputDataReceived += OnOutputDataReceived;
            }

            public string WriteLine(string value)
            {
                _input.WriteLine(value);
                return "";
            }

            private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                
            }

            public void Dispose()
            {
                
            }
        }
    }

    public interface ICommandStream : IDisposable
    {
        string WriteLine(string command);
    }
}