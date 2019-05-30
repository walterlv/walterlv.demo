using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Walterlv.UwpProxyFix
{
    class Program
    {
        static void Main(string[] args)
        {
            var packagePath = Environment.ExpandEnvironmentVariables(@"%LocalAppData%\Packages");
            foreach (var directory in new DirectoryInfo(packagePath).GetDirectories())
            {
                packagePath = $@"CheckNetIsolation.exe LoopbackExempt -a -n=""{directory.Name}""";
                var output = Control(packagePath);
                Console.WriteLine(output);
            }
        }

        private static string Control(string command)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                }
            };
            process.Start();

            process.StandardInput.WriteLine(command + "&exit");
            process.StandardInput.AutoFlush = true;
            var output = process.StandardOutput.ReadToEnd();
            output += process.StandardError.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return output + "\r\n";
        }
    }
}
