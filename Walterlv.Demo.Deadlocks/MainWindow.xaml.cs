using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.SetMinThreads(100, 100);
            for (var i = 0; i < 100; i++)
            {
                DoAsync();
            }
        }

        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);

        private void Do()
        {
            _resetEvent.WaitOne();

            try
            {
                DoCore();
            }
            finally
            {
                _resetEvent.Set();
            }
        }

        private void DoCore()
        {

        }

        private async Task DoAsync()
        {
            _resetEvent.WaitOne();

            try
            {
                await DoCoreAsync();
            }
            finally
            {
                _resetEvent.Set();
            }
        }

        private async Task DoCoreAsync()
        {
            await Task.Run(() => { });
        }
    }

    class Walterlv
    {
        public Walterlv()
        {
            // 等待一段时间，是为了给我么的测试程序一个准确的时机。
            Thread.Sleep(100);

            // Invoke 到主线程执行，里面什么都不做是为了证明绝不是里面代码带来的影响。
            Application.Current.Dispatcher.Invoke(() =>
            {

            });
        }
    }
}
