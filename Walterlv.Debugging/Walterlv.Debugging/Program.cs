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
                Console.WriteLine("Walterlv child application");
                Console.WriteLine(string.Join(Environment.NewLine, args));
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Walterlv main application");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, "--child"),
                };
                process.Start();
                process.WaitForExit();
            }
        }

        //private static void AttachDebugger()
        //{
        //    int ProcId = 5044; // valid process-id
        //    EnvDTE80.DTE2 dte2 =
        //        (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.10.0");
        //    foreach (EnvDTE80.Process2 proc in dte2.Debugger.LocalProcesses)
        //    {
        //        if (proc.ProcessID == ProcId)
        //        {
        //            proc.Attach2();
        //            break;
        //        }
        //    }
        //}
    }
}