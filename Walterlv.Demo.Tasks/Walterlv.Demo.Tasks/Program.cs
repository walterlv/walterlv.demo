using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Walterlv.Demo.Tasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "walterlv task demo";

            for (var i = 8; i < 9; i++)
            {
                //Console.Write($"执行 {i.ToString(CultureInfo.InvariantCulture).PadLeft(2, ' ')} 个");
                var elapsed = await TimeTest(15);
                Console.WriteLine($"耗时: {elapsed}");
            }

            Console.Read();
        }

        private static async Task<TimeSpan> TimeTest(int count)
        {
            var stopwatch = Stopwatch.StartNew();

            var task = Enumerable.Range(0, count).Select(i => Task.Run(() => LongTimeTask(i).Result)).ToList();
            await Task.WhenAll(task);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static async Task<int> LongTimeTask(int index)
        {
            return await Task.Run(() => 1);
        }
    }

    public class A : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
    }
}
