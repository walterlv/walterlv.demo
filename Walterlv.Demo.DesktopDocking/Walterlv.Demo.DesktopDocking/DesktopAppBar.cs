using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
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
            public IntPtr lParam;
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

        private class AppBarWindow
        {
            public int CallbackId { get; set; }
            public bool IsRegistered { get; set; }
            public Window Window { get; set; }
            public AppBarEdge Edge { get; set; }
            public WindowStyle OriginalStyle { get; set; }
            public Rect RestoreBounds { get; set; }
            public ResizeMode OriginalResizeMode { get; set; }

            public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam,
                IntPtr lParam, ref bool handled)
            {
                if (msg == CallbackId)
                {
                    if (wParam.ToInt32() == (int) ABNotify.ABN_POSCHANGED)
                    {
                        ABSetPos(Edge, Window);
                        handled = true;
                    }
                }

                return IntPtr.Zero;
            }
        }

        private static readonly Dictionary<Window, AppBarWindow> RegisteredWindowInfo
            = new Dictionary<Window, AppBarWindow>();

        private static AppBarWindow GetRegisterInfo(Window window)
        {
            if (!RegisteredWindowInfo.TryGetValue(window, out var info))
            {
                info = new AppBarWindow
                {
                    CallbackId = 0,
                    Window = window,
                    IsRegistered = false,
                    Edge = AppBarEdge.Top,
                    OriginalStyle = window.WindowStyle,
                    RestoreBounds = window.RestoreBounds,
                    OriginalResizeMode = window.ResizeMode,
                };
                RegisteredWindowInfo.Add(window, info);
            }

            return info;
        }

        private static void RestoreWindow(Window appbarWindow)
        {
            var info = GetRegisterInfo(appbarWindow);

            appbarWindow.WindowStyle = info.OriginalStyle;
            appbarWindow.ResizeMode = info.OriginalResizeMode;
            appbarWindow.Topmost = false;

            appbarWindow.Dispatcher.InvokeAsync(
                () => DoResize(appbarWindow, info.RestoreBounds), DispatcherPriority.ApplicationIdle);
        }

        public static void SetAppBar(Window appbarWindow, AppBarEdge edge)
        {
            var info = GetRegisterInfo(appbarWindow);
            info.Edge = edge;

            var data = new APPBARDATA();
            data.cbSize = Marshal.SizeOf(data);
            data.hWnd = new WindowInteropHelper(appbarWindow).Handle;

            if (data.hWnd == IntPtr.Zero)
            {
                throw new ArgumentException("请在 Window.SourceInitialized 事件引发之后再设置停靠属性。", nameof(appbarWindow));
            }

            if (edge == AppBarEdge.None)
            {
                if (info.IsRegistered)
                {
                    SHAppBarMessage((int) ABMsg.ABM_REMOVE, ref data);
                    info.IsRegistered = false;
                }

                RestoreWindow(appbarWindow);
                return;
            }

            if (!info.IsRegistered)
            {
                info.IsRegistered = true;
                info.CallbackId = RegisterWindowMessage("AppBarMessage");
                data.uCallbackMessage = info.CallbackId;

                var ret = SHAppBarMessage((int) ABMsg.ABM_NEW, ref data);

                var source = HwndSource.FromHwnd(data.hWnd);
                source.AddHook(info.WndProc);
            }

            appbarWindow.WindowStyle = WindowStyle.None;
            appbarWindow.ResizeMode = ResizeMode.NoResize;
            appbarWindow.Topmost = true;

            ABSetPos(info.Edge, appbarWindow);
        }

        private delegate void ResizeDelegate(Window appbarWindow, Rect rect);

        private static void DoResize(Window appbarWindow, Rect rect)
        {
            appbarWindow.Width = rect.Width;
            appbarWindow.Height = rect.Height;
            appbarWindow.Top = rect.Top;
            appbarWindow.Left = rect.Left;
        }

        private static void ABSetPos(AppBarEdge edge, Window appbarWindow)
        {
            var barData = new APPBARDATA();
            barData.cbSize = Marshal.SizeOf(barData);
            barData.hWnd = new WindowInteropHelper(appbarWindow).Handle;
            barData.uEdge = (int) edge;

            if (barData.uEdge == (int) AppBarEdge.Left || barData.uEdge == (int) AppBarEdge.Right)
            {
                barData.rc.top = 0;
                barData.rc.bottom = (int) SystemParameters.PrimaryScreenHeight;
                if (barData.uEdge == (int) AppBarEdge.Left)
                {
                    barData.rc.left = 0;
                    barData.rc.right = (int) Math.Round(appbarWindow.ActualWidth);
                }
                else
                {
                    barData.rc.right = (int) SystemParameters.PrimaryScreenWidth;
                    barData.rc.left = barData.rc.right - (int) Math.Round(appbarWindow.ActualWidth);
                }
            }
            else
            {
                barData.rc.left = 0;
                barData.rc.right = (int) SystemParameters.PrimaryScreenWidth;
                if (barData.uEdge == (int) AppBarEdge.Top)
                {
                    barData.rc.top = 0;
                    barData.rc.bottom = (int) Math.Round(appbarWindow.ActualHeight);
                }
                else
                {
                    barData.rc.bottom = (int) SystemParameters.PrimaryScreenHeight;
                    barData.rc.top = barData.rc.bottom - (int) Math.Round(appbarWindow.ActualHeight);
                }
            }

            SHAppBarMessage((int) ABMsg.ABM_QUERYPOS, ref barData);
            SHAppBarMessage((int) ABMsg.ABM_SETPOS, ref barData);

            var rect = new Rect(barData.rc.left, barData.rc.top,
                barData.rc.right - barData.rc.left, barData.rc.bottom - barData.rc.top);
            //This is done async, because WPF will send a resize after a new appbar is added.  
            //if we size right away, WPFs resize comes last and overrides us.
            appbarWindow.Dispatcher.InvokeAsync(
                () => DoResize(appbarWindow, rect), DispatcherPriority.ApplicationIdle);
        }
    }
}
