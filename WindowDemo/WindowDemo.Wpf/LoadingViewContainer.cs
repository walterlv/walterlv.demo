using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Walterlv.Demo.Runtime;
using Walterlv.Demo.Win32;

namespace Walterlv.Demo
{
    public class LoadingViewContainer : Border
    {
        static LoadingViewContainer()
        {
            LoadingDispatcherOperation = UIDispatcher.RunNewAsync("LoadingView");
        }

        private static readonly DispatcherAsyncOperation<Dispatcher> LoadingDispatcherOperation;

        public LoadingViewContainer()
        {
            Loaded += OnLoaded;
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (Equals(_isLoading, value)) return;
                _isLoading = value;
                if (value)
                {
                    ShowLoading().ConfigureAwait(false);
                }
                else
                {
                    CloseLoading().ConfigureAwait(false);
                }
            }
        }

        private async Task ShowLoading()
        {
            var tag = Tag?.ToString();
            var dispatcher = await LoadingDispatcherOperation;
            await dispatcher.InvokeAsync(() =>
            {
                _loadingWindow = new Window
                {
                    Content = tag,
                    //BorderBrush = Brushes.DodgerBlue,
                    //BorderThickness = new Thickness(8),
                    //Background = Brushes.Teal,
                    //WindowStyle = WindowStyle.None,
                    //ResizeMode = ResizeMode.NoResize,
                    //Content = new TextBlock
                    //{
                    //    Text = "walterlv.github.io",
                    //    HorizontalAlignment = HorizontalAlignment.Center,
                    //    VerticalAlignment = VerticalAlignment.Center,
                    //    Foreground = Brushes.White,
                    //    FontSize = 24,
                    //}
                };
                _loadingWindow.SourceInitialized += LoadingWindow_SourceInitialized;
                _loadingWindow.Show();
            });
        }

        private async Task CloseLoading()
        {
            var dispatcher = await LoadingDispatcherOperation;
            await dispatcher.InvokeAsync(() =>
            {
                _loadingWindow?.Close();
                _loadingWindow = null;
            });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var hwnd = (HwndSource) PresentationSource.FromVisual(this);
            _parentHwnd = hwnd;
        }

        private void LoadingWindow_SourceInitialized(object sender, EventArgs e)
        {
            if (_loadingWindow == null)
            {
                // 说明在显示完成之前，已经关闭了窗口。
                return;
            }

            if (_parentHwnd == null)
            {
                // 说明试图显示 Loading 之前，主窗口尚未完成初始化。
                return;
            }

            var childHandle = new WindowInteropHelper(_loadingWindow).Handle;
            UnmanagedMethods.SetParent(childHandle, _parentHwnd.Handle);
            UnmanagedMethods.MoveWindow(childHandle, 0, 0, 300, 300, true);
        }

        private Window _loadingWindow;
        private HwndSource _parentHwnd;
        private bool _isLoading;
    }
}
