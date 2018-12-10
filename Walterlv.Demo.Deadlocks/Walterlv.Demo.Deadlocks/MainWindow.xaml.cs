using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.Demo.Deadlocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(true);
        private int _count;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.SetMinThreads(100, 100);

            // 全部在后台线程，不会死锁。
            for (var i = 0; i < 100; i++)
            {
                Task.Run(() => DoAsync());
            }

            // 主线程执行与后台线程并发竞争，也不会死锁。
            //for (var i = 0; i < 100; i++)
            //{
            //    DoAsync();
            //}
        }

        private void Do()
        {
            _resetEvent.WaitOne();

            try
            {
                // 这个 ++ 在安全的线程上下文中，所以不需要使用 Interlocked.Increment(ref _count);
                _count++;
                DoCore();
            }
            finally
            {
                _resetEvent.Set();
            }
        }

        private void DoCore()
        {
            Console.WriteLine($"[{_count.ToString().PadLeft(3, ' ')}] walterlv is a 逗比");
        }

        private async Task DoAsync()
        {
            _resetEvent.WaitOne();

            try
            {
                _count++;
                await DoCoreAsync();
            }
            finally
            {
                _resetEvent.Set();
            }
        }

        private async Task DoCoreAsync()
        {
            await Task.Run(async () =>
            {
                Console.WriteLine($"[{_count.ToString().PadLeft(3, ' ')}] walterlv is a 逗比");
            });
        }
    }
}
