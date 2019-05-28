using System;
using System.Diagnostics;
using System.IO;

namespace Walterlv.Demo.StartingElapse
{
    class Program
    {
        static void Main(string[] args)
        {
            var time1 = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();
            var startTime = Process.GetCurrentProcess().StartTime;
            stopwatch.Stop();
            var time2 = DateTime.Now;
            Console.WriteLine($@"{startTime.Ticks} - {startTime:O}
{time1.Ticks} - {time1:O}
{time2.Ticks} - {time2:O}
{stopwatch.ElapsedTicks} - {stopwatch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
