using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Walterlv.Annotations;
using Walterlv.Demo;
using Walterlv.Demo.Utils.Threading;
using DispatcherDictionary = System.Collections.Concurrent.ConcurrentDictionary<System.Windows.Threading.Dispatcher, Walterlv.Demo.Utils.Threading.DispatcherAsyncOperation<System.Windows.Threading.Dispatcher>>;

namespace Walterlv.Windows
{
    [ContentProperty(nameof(Child))]
    public class AsyncBox : FrameworkElement
    {
        /// <summary>
        /// 保存外部 UI 线程和与其关联的异步 UI 线程。
        /// 例如主 UI 线程对应一个 AsyncBox 专用的 UI 线程；外面可能有另一个 UI 线程，那么对应另一个 AsyncBox 专用的 UI 线程。
        /// </summary>
        private static readonly DispatcherDictionary RelatedAsyncDispatchers = new DispatcherDictionary();

        [CanBeNull]
        private UIElement _child;

        [CanBeNull]
        private Type _loadingViewType;

        public AsyncBox()
        {
            Loaded += OnLoaded;
        }

        [CanBeNull]
        public UIElement Child
        {
            get => _child;
            set
            {
                if (Equals(_child, value)) return;

                _child = value;
            }
        }

        [NotNull]
        public Type LoadingViewType
        {
            get
            {
                if (_loadingViewType == null)
                {
                    throw new InvalidOperationException(
                        $"在 {nameof(AsyncBox)} 显示之前，必须先为 {nameof(LoadingViewType)} 设置一个 {nameof(UIElement)} 作为 Loading 视图。");
                }

                return _loadingViewType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(LoadingViewType));
                }

                if (_loadingViewType != null)
                {
                    throw new ArgumentException($"{nameof(LoadingViewType)} 只允许被设置一次。", nameof(value));
                }

                _loadingViewType = value;
            }
        }

        /// <summary>
        /// 返回一个可等待的用于显示异步 UI 的后台 UI 线程调度器。
        /// </summary>
        [NotNull]
        private DispatcherAsyncOperation<Dispatcher> GetAsyncDispatcherAsync() => RelatedAsyncDispatchers.GetOrAdd(
            Dispatcher, dispatcher => UIDispatcher.RunNewAsync("AsyncBox"));

        [NotNull]
        private UIElement CreateLoadingView()
        {
            var instance = Activator.CreateInstance(LoadingViewType);
            if (instance is UIElement element)
            {
                return element;
            }

            throw new InvalidOperationException($"{LoadingViewType} 必须是 {nameof(UIElement)} 类型");
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            var dispatcher = await GetAsyncDispatcherAsync();
            var view = await dispatcher.InvokeAsync(CreateLoadingView);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }
    }
}
