using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Walterlv.Debugging
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                Console.WriteLine("Child application");
                Console.WriteLine(string.Join(Environment.NewLine, args));
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Main application");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, "--child"),
                };
                process.Start();
                var processArgs = process.GetCommandLineArgs();
                Console.WriteLine(processArgs);
                process.WaitForExit();
            }
        }
    }

    public static class ProcessExtensions
    {
        public static string GetCommandLineArgs(this Process process)
        {
            if (process is null) throw new ArgumentNullException(nameof(process));

            try
            {
                return GetCommandLineArgsCore();
            }
            catch (Win32Exception ex) when ((uint) ex.ErrorCode == 0x80004005)
            {
                // 没有对该进程的安全访问权限。
                return string.Empty;
            }
            catch (InvalidOperationException)
            {
                // 进程已退出。
                return string.Empty;
            }

            string GetCommandLineArgsCore()
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
                using (var objects = searcher.Get())
                {
                    var @object = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                    return @object?["CommandLine"]?.ToString() ?? "";
                }
            }
        }
    }
}
