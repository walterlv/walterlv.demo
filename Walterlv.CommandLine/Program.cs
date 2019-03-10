using System;
using System.Diagnostics;
using Cvte.Cli;

namespace Walterlv
{
    class Program
    {
        static void Main(string[] args)
        {
            //var stopwatch = new Stopwatch();
            for (var i = 0; i < 1000000; i++)
            {
                //stopwatch.Restart();

                var commandLine = CommandLine.Parse(args, urlProtocol: "walterlv");
                var option = commandLine.Handle(
                    options => 0,
                    options => 0,
                    new SelfWrittenEditOptionsParser(),
                    new SelfWrittenPrintOptionsParser());

                //stopwatch.Stop();
                //Console.Write(option.ToString()[0]);
                //Console.CursorLeft = 0;
                //Console.WriteLine($"{i} - {stopwatch.Elapsed.Ticks} ticks & {stopwatch.Elapsed.TotalMilliseconds} ms");
            }

            if (Debugger.IsAttached && !Console.IsInputRedirected)
            {
                Console.ReadLine();
            }
        }
    }
}