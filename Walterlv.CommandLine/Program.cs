using System;
using System.Diagnostics;
using Walterlv.Framework;

namespace Walterlv
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            for (var i = 0; i < 100; i++)
            {
                stopwatch.Restart();
                var commandLine = CommandLine.Parse(args, urlProtocol: "walterlv");
                var option = commandLine.As<Options>(new OptionsParser());
                stopwatch.Stop();
                Console.Write(option.ToString()[0]);
                Console.CursorLeft = 0;
                Console.WriteLine($"{i} - {stopwatch.Elapsed.Ticks} ticks & {stopwatch.Elapsed.TotalMilliseconds} ms");
            }
            Console.ReadLine();
        }
    }
}
