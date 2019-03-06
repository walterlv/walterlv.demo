using System;
using BenchmarkDotNet.Running;

namespace Walterlv.CommandLine.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CommandLineParserTest>();
            Console.ReadLine();
        }
    }
}
