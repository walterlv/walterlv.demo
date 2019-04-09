using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.FastKeyValue
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var test = new Program();

            Parallel.ForEach(Enumerable.Range(0, 32), i =>
            {
                test.ReenterAsync().Wait();
            });

            Console.Read();

            //var fast = new FastKeyValue();
            //var watch = new Stopwatch();
            //watch.Start();

            //for (var i = 0; i < 1000; i++)
            //{
            //    await fast.LoadAsync(@"C:\Users\lvyi\Desktop\keyvalue.fkv.txt");
            //}

            //watch.Stop();
            //Console.WriteLine($"用时：{watch.Elapsed}");
        }

        /// <summary>
        /// 设置此值为 true 表示申请发起动作。
        /// </summary>
        private bool _isRequired;

        private bool _isRunning;

        private object _locker;

        private async Task ReenterAsync()
        {
            Console.WriteLine($"Thread Call: {Thread.CurrentThread.ManagedThreadId}");

            if (_isRequired)
            {
                return;
            }

            while (true)
            {
                _isRequired = true;

                await Task.Delay(110).ConfigureAwait(false);

                if (_isRunning || !_isRequired) return;

                _isRunning = true;

                try
                {
                    Enter();
                    _isRequired = false;
                    _isRunning = false;
                }
                catch
                {
                    _isRequired = true;
                    _isRunning = false;
                }

                if (_isRequired)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        private void Enter()
        {
            Console.WriteLine($"Entered: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
