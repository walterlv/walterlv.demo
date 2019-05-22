using System;
using System.Diagnostics;

namespace Walterlv.Demo.AliveProcesses
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.HasExited)
                    {
                        Console.WriteLine($"{process.ProcessName} - HasExited");
                    }
                }
                catch (Exception ex)
                {
                }
            }

            Console.ReadLine();
        }
    }
}
