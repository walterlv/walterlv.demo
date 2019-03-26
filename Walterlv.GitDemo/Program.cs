using System;
using System.Diagnostics;
using System.Text;

namespace Walterlv.GitDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine();
        }

        private static string RunCommand(string executablePath, string arguments,
            string workingDirectory)
        {
            var info = new ProcessStartInfo("git", "status")
            {
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
            };
            var builder = new StringBuilder();
            var process = new Process
            {
                StartInfo = info,
            };
            process.OutputDataReceived += (sender, args) => builder.Append(args.Data);
            process.Start();
            process.WaitForExit();
            return builder.ToString();
        }
    }
}
