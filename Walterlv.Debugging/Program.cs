using System;
using System.Diagnostics;
using System.Linq;

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
                process.WaitForExit();
            }
        }
    }
}
