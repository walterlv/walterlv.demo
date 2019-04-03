using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Walterlv.Demo.TreePerformance
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CachePanel.ChildrenCacheStrategy = new WeightCountLimitCache();
            ContentRendered += OnContentRendered;
        }

        private readonly Random _random = new Random();

        private void OnContentRendered(object sender, EventArgs e)
        {
            ContentRendered -= OnContentRendered;
            RunCachePanelPerformanceTest();
        }

        private void RunCachePanelPerformanceTest()
        {
            var childCount = 50;
            var contentCount = 1000;

            var grids = new UniformGrid[childCount];
            for (var i = 0; i < childCount; i++)
            {
                grids[i] = new UniformGrid();
            }

            var buttons = new Button[childCount, contentCount];
            for (var i = 0; i < childCount; i++)
            {
                for (var j = 0; j < contentCount; j++)
                {
                    buttons[i, j] = new Button();
                }
            }

            LogElapse(() =>
            {
                for (var i = 0; i < childCount; i++)
                {
                    var grid = grids[i];
                    for (var j = 0; j < contentCount; j++)
                    {
                        grid.Children.Add(buttons[i, j]);
                    }

                    CachePanel.Children.Add(grid);
                }
            }, "添加元素");
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            LogElapse(() =>
            {
                var index = _random.Next(50);
                CachePanel.CurrentChild = CachePanel.Children[index];
            }, "切换孩子");
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
