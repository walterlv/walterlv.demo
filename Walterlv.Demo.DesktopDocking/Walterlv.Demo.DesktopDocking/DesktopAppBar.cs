using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable EnumUnderlyingTypeIsInt
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Walterlv.Demo.DesktopDocking
{
    /// <summary>
    /// 表示窗口停靠到桌面上时的边缘方向。
    /// </summary>
    public enum AppBarEdge
    {
        /// <summary>
        /// 窗口停靠到桌面的左边。
        /// </summary>
        Left = 0,

        /// <summary>
        /// 窗口停靠到桌面的顶部。
        /// </summary>
        Top,

        /// <summary>
        /// 窗口停靠到桌面的右边。
        /// </summary>
        Right,

        /// <summary>
        /// 窗口停靠到桌面的底部。
        /// </summary>
        Bottom,

        /// <summary>
        /// 窗口不停靠到任何方向，而是成为一个普通窗口占用剩余的可用空间（工作区）。
        /// </summary>
        None
    }

    /// <summary>
    /// 提供将窗口停靠到桌面某个方向的能力。
    /// </summary>
    public class DesktopAppBar
    {
        public static readonly DependencyProperty AppBarProperty = DependencyProperty.RegisterAttached(
            "AppBar", typeof(AppBarEdge), typeof(DesktopAppBar),
            new PropertyMetadata(AppBarEdge.None, OnAppBarEdgeChanged));

        public static AppBarEdge GetAppBar(Window window) => (AppBarEdge) window.GetValue(AppBarProperty);

        public static void SetAppBar(Window window, AppBarEdge value) => window.SetValue(AppBarProperty, value);

        private static readonly DependencyProperty AppBarProcessorProperty = DependencyProperty.RegisterAttached(
            "AppBarProcessor", typeof(AppBarWindowProcessor), typeof(DesktopAppBar), new PropertyMetadata(null));

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        private static void OnAppBarEdgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (AppBarEdge) e.OldValue;
            var newValue = (AppBarEdge) e.NewValue;
            var oldEnabled = oldValue is AppBarEdge.Left
                             || oldValue is AppBarEdge.Top
                             || oldValue is AppBarEdge.Right
                             || oldValue is AppBarEdge.Bottom;
            var newEnabled = newValue is AppBarEdge.Left
                             || newValue is AppBarEdge.Top
                             || newValue is AppBarEdge.Right
                             || newValue is AppBarEdge.Bottom;
            if (oldEnabled && !newEnabled)
            {
                var processor = (AppBarWindowProcessor) d.GetValue(AppBarProcessorProperty);
                processor.Detach();
            }
            else if (!oldEnabled && newEnabled)
            {
                var processor = new AppBarWindowProcessor((Window) d);
                d.SetValue(AppBarProcessorProperty, processor);
                processor.Attach(newValue);
            }
            else if (oldEnabled && newEnabled)
            {
                var processor = (AppBarWindowProcessor) d.GetValue(AppBarProcessorProperty);
                processor.Update(newValue);
            }
        }

        /// <summary>
        /// 包含对 <see cref="Window"/> 进行操作以便使其成为一个桌面停靠窗口的能力。
        /// </summary>
        private class AppBarWindowProcessor
        {
            /// <summary>
            /// 创建 <see cref="AppBarWindowProcessor"/> 的新实例。
            /// </summary>
            /// <param name="window">需要成为停靠窗口的 <see cref="Window"/> 的实例。</param>
            public AppBarWindowProcessor(Window window)
            {
                var source = (HwndSource) PresentationSource.FromVisual(window);
                _hwndSource = source ?? throw new InvalidOperationException(
                                  "在 Window.SourceInitialized 事件之前和窗口关闭之后无法设置停靠属性。");

                _window = window;

                _callbackId = RegisterWindowMessage("AppBarMessage");
                _restoreStyle = window.WindowStyle;
                _restoreBounds = window.RestoreBounds;
                _restoreResizeMode = window.ResizeMode;
                _restoreTopmost = window.Topmost;

                _window.Closed += OnClosed;
            }

            private readonly Window _window;
            private readonly HwndSource _hwndSource;
            private readonly int _callbackId;
            private readonly WindowStyle _restoreStyle;
            private readonly Rect _restoreBounds;
            private readonly ResizeMode _restoreResizeMode;
            private readonly bool _restoreTopmost;

            private AppBarEdge Edge { get; set; }

            /// <summary>
            /// 在窗口关闭之后，需要恢复窗口设置过的停靠属性。
            /// </summary>
            private void OnClosed(object sender, EventArgs e)
            {
                _window.Closed -= OnClosed;
                _window.ClearValue(AppBarProperty);
            }

            /// <summary>
            /// 使一个窗口开始成为桌面停靠窗口，并开始处理窗口停靠消息。
            /// </summary>
            /// <param name="value">停靠方向。</param>
            public void Attach(AppBarEdge value)
            {
                var data = new APPBARDATA();
                data.cbSize = Marshal.SizeOf(data);
                data.hWnd = _hwndSource.Handle;

                data.uCallbackMessage = _callbackId;
                SHAppBarMessage((int) ABMsg.ABM_NEW, ref data);
                _hwndSource.AddHook(WndProc);

                Update(value);
            }

            /// <summary>
            /// 更新一个窗口的停靠方向。
            /// </summary>
            /// <param name="value">停靠方向。</param>
            public void Update(AppBarEdge value)
            {
                Edge = value;

                _window.WindowStyle = WindowStyle.None;
                _window.ResizeMode = ResizeMode.NoResize;
                _window.Topmost = true;

                ABSetPos(_window, value);
            }

            /// <summary>
            /// 使一个窗口从桌面停靠窗口恢复成普通窗口。
            /// </summary>
            public void Detach()
            {
                var data = new APPBARDATA();
                data.cbSize = Marshal.SizeOf(data);
                data.hWnd = _hwndSource.Handle;

                SHAppBarMessage((int) ABMsg.ABM_REMOVE, ref data);

                _window.WindowStyle = _restoreStyle;
                _window.ResizeMode = _restoreResizeMode;
                _window.Topmost = _restoreTopmost;

                _window.Dispatcher.InvokeAsync(
                    () => Resize(_window, _restoreBounds), DispatcherPriority.ContextIdle);
            }

            private IntPtr WndProc(IntPtr hwnd, int msg,
                IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                if (msg == _callbackId)
                {
                    if (wParam.ToInt32() == (int) ABNotify.ABN_POSCHANGED)
                    {
                        ABSetPos(_window, Edge);
                        handled = true;
                    }
                }

                return IntPtr.Zero;
            }

            private static void Resize(Window window, Rect bounds)
            {
                window.Left = bounds.Left;
                window.Top = bounds.Top;
                window.Width = bounds.Width;
                window.Height = bounds.Height;
            }

            private static void ABSetPos(Window window, AppBarEdge edge)
            {
                var data = new APPBARDATA();
                data.cbSize = Marshal.SizeOf(data);
                data.hWnd = new WindowInteropHelper(window).Handle;
                data.uEdge = (int) edge;

                if (data.uEdge == (int) AppBarEdge.Left || data.uEdge == (int) AppBarEdge.Right)
                {
                    data.rc.top = 0;
                    data.rc.bottom = (int) SystemParameters.PrimaryScreenHeight;
                    if (data.uEdge == (int) AppBarEdge.Left)
                    {
                        data.rc.left = 0;
                        data.rc.right = (int) Math.Round(window.ActualWidth);
                    }
                    else
                    {
                        data.rc.right = (int) SystemParameters.PrimaryScreenWidth;
                        data.rc.left = data.rc.right - (int) Math.Round(window.ActualWidth);
                    }
                }
                else
                {
                    data.rc.left = 0;
                    data.rc.right = (int) SystemParameters.PrimaryScreenWidth;
                    if (data.uEdge == (int) AppBarEdge.Top)
                    {
                        data.rc.top = 0;
                        data.rc.bottom = (int) Math.Round(window.ActualHeight);
                    }
                    else
                    {
                        data.rc.bottom = (int) SystemParameters.PrimaryScreenHeight;
                        data.rc.top = data.rc.bottom - (int) Math.Round(window.ActualHeight);
                    }
                }

                SHAppBarMessage((int) ABMsg.ABM_QUERYPOS, ref data);
                SHAppBarMessage((int) ABMsg.ABM_SETPOS, ref data);

                var bounds = new Rect(data.rc.left, data.rc.top,
                    data.rc.right - data.rc.left, data.rc.bottom - data.rc.top);

                window.Dispatcher.InvokeAsync(
                    () => Resize(window, bounds), DispatcherPriority.ContextIdle);
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct APPBARDATA
            {
                public int cbSize;
                public IntPtr hWnd;
                public int uCallbackMessage;
                public int uEdge;
                public RECT rc;
                public readonly IntPtr lParam;
            }

            private enum ABMsg : int
            {
                ABM_NEW = 0,
                ABM_REMOVE,
                ABM_QUERYPOS,
                ABM_SETPOS,
                ABM_GETSTATE,
                ABM_GETTASKBARPOS,
                ABM_ACTIVATE,
                ABM_GETAUTOHIDEBAR,
                ABM_SETAUTOHIDEBAR,
                ABM_WINDOWPOSCHANGED,
                ABM_SETSTATE
            }

            private enum ABNotify : int
            {
                ABN_STATECHANGE = 0,
                ABN_POSCHANGED,
                ABN_FULLSCREENAPP,
                ABN_WINDOWARRANGE
            }

            [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
            private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            private static extern int RegisterWindowMessage(string msg);
        }
    }
}
