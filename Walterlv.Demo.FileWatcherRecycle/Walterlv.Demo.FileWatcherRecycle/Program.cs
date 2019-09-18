using System;
using System.Threading.Tasks;
using Walterlv.IO;

namespace Walterlv.Demo.FileWatcherRecycle
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WatchFileAsync(@"D:\Desktop\README.md");
            while (true)
            {
                await Task.Delay(1000);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Console.WriteLine("已回收内存");
            }
        }

        private static async void WatchFileAsync(string fileName)
        {
            var watcher = new FileWatcher(fileName);
            watcher.Changed += OnFileChanged;
            watcher.WatchAsync();
        }

        private static void OnFileChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] 文件发生改变");
        }
    }
}
