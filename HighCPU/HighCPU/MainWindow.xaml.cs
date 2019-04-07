using System;
using System.Windows;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;

namespace HighCPU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _threadNum = 2;
        private int _threadSleepTime = 1000;
        private int num = 1;

        public MainWindow()
        {
            InitializeComponent();
            _threadNum = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadNum"]);
            _threadSleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadSleepTime"]);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Block.Text = $"已经运行{_threadNum * num++}个线程";
            for (int i = 0; i < _threadNum; i++)
            {
                var thread = new Thread(() =>
                {
                    var num = i;
                    while (true)
                    {
                        Console.WriteLine($"{num}");
                        Thread.Sleep(_threadSleepTime);
                    }
                });
                thread.Start();
            }
        }
    }
}