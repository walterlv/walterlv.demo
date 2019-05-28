using System;
using System.Diagnostics;

namespace Walterlv.Demo.StartingElapse
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var time1 = DateTime.Now;
            var startTime = Process.GetCurrentProcess().StartTime;
            var time2 = DateTime.Now;
            stopwatch.Stop();
            Console.WriteLine($"{time1} - {time2} - {startTime}");
            Console.ReadLine();
        }
    }
}
