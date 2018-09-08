using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Walterlv.Demo;
using Walterlv.Demo.Utils.Threading;
using DispatcherDictionary = System.Collections.Concurrent.ConcurrentDictionary<System.Windows.Threading.Dispatcher, Walterlv.Demo.Utils.Threading.DispatcherAsyncOperation<System.Windows.Threading.Dispatcher>>;

namespace Walterlv.Windows
{
    public class AsyncBox : FrameworkElement
    {
        /// <summary>
        /// 保存外部 UI 线程和与其关联的异步 UI 线程。
        /// 例如主 UI 线程对应一个 AsyncBox 专用的 UI 线程；外面可能有另一个 UI 线程，那么对应另一个 AsyncBox 专用的 UI 线程。
        /// </summary>
        private static readonly DispatcherDictionary RelatedAsyncDispatchers = new DispatcherDictionary();

        public AsyncBox()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 返回一个可等待的用于显示异步 UI 的后台 UI 线程调度器。
        /// </summary>
        private DispatcherAsyncOperation<Dispatcher> GetAsyncDispatcherAsync() => RelatedAsyncDispatchers.GetOrAdd(
            Dispatcher, dispatcher => UIDispatcher.RunNewAsync("AsyncBox"));

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var watch = new Stopwatch();

            watch.Start();
            var dispatcher = await GetAsyncDispatcherAsync();
            watch.Stop();
            var uiDispatcherElapsed = watch.Elapsed;

            watch.Restart();
            var panel = new Grid();
            for (var i = 0; i < 10000; i++)
            {
                panel.Children.Add(new Button());
            }
            watch.Stop();
            var newElapsed = watch.Elapsed;

            watch.Restart();
            panel.Measure(new Size(300, 500));
            panel.Arrange(new Rect(0, 0, 300, 500));
            watch.Stop();
            var layoutElapsed = watch.Elapsed;

            watch.Restart();
            AddVisualChild(panel);
            watch.Stop();
            var visualTreeElapsed = watch.Elapsed;

            watch.Restart();
            AddLogicalChild(panel);
            watch.Stop();
            var logicalTreeElapsed = watch.Elapsed;

            var format = $"线程 {uiDispatcherElapsed}, 创建 {newElapsed}, 布局 {layoutElapsed}, 可视化树 {visualTreeElapsed}, 逻辑树 {logicalTreeElapsed}";
            Console.WriteLine(format);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }
    }
}
