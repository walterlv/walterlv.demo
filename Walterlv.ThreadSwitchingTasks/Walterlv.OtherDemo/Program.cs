using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Walterlv.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(SynchronizationContext.Current?.ToString() ?? "null");
            MyControl my = new MyControl();
            Console.WriteLine(SynchronizationContext.Current?.ToString() ?? "null");

            var task = Task.Run(() => { });
            await task;
            Thread.Sleep(-1);
        }

        public class MyControl : Control
        {
        }
    }
}