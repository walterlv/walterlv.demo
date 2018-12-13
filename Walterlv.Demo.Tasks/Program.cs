using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Demo.Tasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "walterlv task demo";

            var scheduler = TaskScheduler.Default;
            ThreadPool.SetMinThreads(8, 32);

            var task = Enumerable.Range(0, 32).Select(i => Task.Run(() => LongTimeTask(i))).ToList();
            await Task.WhenAll(task);


            Console.Read();
        }

        private static void LongTimeTask(int index)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2, ' ');
            var line = index.ToString().PadLeft(2, ' ');
            Console.WriteLine($"[{line}] [{threadId}] [{DateTime.Now:ss.fff}] 异步任务已开始……");

            // 这一句才是关键，等待。其他代码只是为了输出。
            var client = new WebClient();
            client.DownloadFile("https://qd.myapp.com/myapp/qqteam/pcqq/QQ9.0.8_1.exe", $@"C:\Users\lvyi\Desktop\测试{index}.x");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{line}] [{threadId}] [{DateTime.Now:ss.fff}] 异步任务已结束……");
            Console.ForegroundColor = ConsoleColor.White;
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
