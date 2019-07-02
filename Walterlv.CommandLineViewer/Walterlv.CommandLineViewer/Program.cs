using System;
using System.Diagnostics;
using System.IO;

namespace Walterlv.CommandLineViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // 输出命令行。
            var argsLine = string.Join(" ", args);
            Console.Write(argsLine);

            // 将命令行转发给原程序。
            var exe = Process.GetCurrentProcess().MainModule.FileName;
            var originalExe = $"{Path.GetFileNameWithoutExtension(exe)}.origin{Path.GetExtension(exe)}";
            Process.Start(originalExe, argsLine);
            Console.ReadLine();
        }
    }
}
