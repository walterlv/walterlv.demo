using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Walterlv.Demo.Threading
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Threading Demo by walterlv.com";

            var stopwatch = Stopwatch.StartNew();
            await Task.Delay(0);
            var elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Task.Delay(0) : {elapsed}");

            stopwatch.Restart();
            await Task.Delay(1);
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Task.Delay(1) : {elapsed}");

            stopwatch.Restart();
            await Task.Delay(15);
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Task.Delay(15): {elapsed}");

            stopwatch.Restart();
            await Task.Delay(16);
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Task.Delay(16): {elapsed}");

            stopwatch.Restart();
            await Task.Yield();
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Task.Yield()  : {elapsed}");

            stopwatch.Restart();
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Nothing       : {elapsed}");

            Console.Read();
        }
    }
}
