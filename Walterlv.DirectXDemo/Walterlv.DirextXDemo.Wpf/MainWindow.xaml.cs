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

namespace Walterlv.DirextXDemo.Wpf
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

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Console.Title = "walterlv task demo";

            var task = Enumerable.Range(0, 10).Select(i => Task.Run(() => LongTimeTask(i))).ToList();
            await Task.WhenAll(task);
        }

            private static void LongTimeTask(int index)
            {
                var threadId = Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2, ' ');
                var line = index.ToString().PadLeft(2, ' ');
                Console.WriteLine($"[{line}] [{threadId}] [{DateTime.Now:ss.fff}] 异步任务已开始……");

                // 这一句才是关键，等待。其他代码只是为了输出。
                Thread.Sleep(5000);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{line}] [{threadId}] [{DateTime.Now:ss.fff}] 异步任务已结束……");
                Console.ForegroundColor = ConsoleColor.White;
            }
    }
}
