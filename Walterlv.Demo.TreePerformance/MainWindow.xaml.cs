using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Walterlv.Demo.TreePerformance
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += OnContentRendered;
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            ContentRendered -= OnContentRendered;
            RunCachePanelPerformanceTest();
        }

        private void RunCachePanelPerformanceTest()
        {
            const int count = 1000;
            LogElapse(() =>
            {
                for (var i = 0; i < count; i++)
                {
                    CachePanel.Children.Add(new Button());
                }
            }, "添加元素");
        }

        private void LogElapse(Action action, string text)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                action();
            }
            finally
            {
                watch.Stop();
                Log($"{text} {watch.Elapsed}");
            }
        }

        private void Log(string text)
        {
            LogTextBlock.Text += $"{text}{Environment.NewLine}";
        }
    }
}
