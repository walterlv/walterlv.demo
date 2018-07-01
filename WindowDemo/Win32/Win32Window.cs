using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Walterlv.Demo.Win32
{
    public class Win32Window : IDisposable
    {
        public void Show()
        {
            var wndProcDelegate = new UnmanagedMethods.WndProc(WndProc);
            var defaultCursor = UnmanagedMethods.LoadCursor(
                IntPtr.Zero, new IntPtr((int) UnmanagedMethods.Cursor.IDC_ARROW));
            var wndClassEx = new UnmanagedMethods.WNDCLASSEX
            {
                cbSize = Marshal.SizeOf<UnmanagedMethods.WNDCLASSEX>(),
                style = 0,
                lpfnWndProc = wndProcDelegate,
                hInstance = UnmanagedMethods.GetModuleHandle(null),
                hCursor = defaultCursor,
                hbrBackground = IntPtr.Zero,
                lpszClassName = "WalterlvSplashWindow"
            };
            var atom = UnmanagedMethods.RegisterClassEx(ref wndClassEx);
            var hwnd = UnmanagedMethods.CreateWindowEx(
                0,
                atom,
                null,
                (int) (UnmanagedMethods.WindowStyles.WS_OVERLAPPEDWINDOW | UnmanagedMethods.WindowStyles.WS_EX_DLGMODALFRAME),
                UnmanagedMethods.CW_USEDEFAULT,
                UnmanagedMethods.CW_USEDEFAULT,
                UnmanagedMethods.CW_USEDEFAULT,
                UnmanagedMethods.CW_USEDEFAULT,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);
            UnmanagedMethods.ShowWindow(hwnd, UnmanagedMethods.ShowWindowCommand.Restore);
        }

        public void Loop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                UnmanagedMethods.GetMessage(out var msg, IntPtr.Zero, 0, 0);
                UnmanagedMethods.TranslateMessage(ref msg);
                UnmanagedMethods.DispatchMessage(ref msg);
            }
        }

        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            //switch (msg)
            //{
            //    case WM_CREATE:
            //        hBitmap = (HBITMAP)LoadImage(hInst, L"c:\\test.bmp", IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
            //        break;
            //    case WM_PAINT:
            //        PAINTSTRUCT ps;
            //        HDC hdc;
            //        BITMAP bitmap;
            //        HDC hdcMem;
            //        HGDIOBJ oldBitmap;

            //        hdc = BeginPaint(hWnd, &ps);

            //        hdcMem = CreateCompatibleDC(hdc);
            //        oldBitmap = SelectObject(hdcMem, hBitmap);

            //        GetObject(hBitmap, sizeof(bitmap), &bitmap);
            //        BitBlt(hdc, 0, 0, bitmap.bmWidth, bitmap.bmHeight, hdcMem, 0, 0, SRCCOPY);

            //        SelectObject(hdcMem, oldBitmap);
            //        DeleteDC(hdcMem);

            //        EndPaint(hWnd, &ps);
            //        break;
            //    case WM_DESTROY:
            //        DeleteObject(hBitmap);
            //        PostQuitMessage(0);
            //        break;
            //}
            return UnmanagedMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        public void Dispose()
        {
        //    _framebuffer?.Dispose();
        //    _framebuffer = null;
        //    if (_hwnd != IntPtr.Zero)
        //    {
        //        UnmanagedMethods.DestroyWindow(_hwnd);
        //        _hwnd = IntPtr.Zero;
        //    }
        //    if (_className != null)
        //    {
        //        UnmanagedMethods.UnregisterClass(_className, UnmanagedMethods.GetModuleHandle(null));
        //        _className = null;
        //    }
        }
    }
}
