using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace Walterlv.CommandLine.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CommandLineParserTest>();

            if (Debugger.IsAttached && !Console.IsInputRedirected)
            {
                Console.ReadLine();
            }
        }
    }
}
