using System;
using System.Runtime.InteropServices;
using System.Threading;
using static Walterlv.Demo.Win32.UnmanagedMethods;

namespace Walterlv.Demo
{
    public class Win32Window : IDisposable
    {
        public void Show()
        {
            var wndProcDelegate = new WndProc(WndProc);
            var defaultCursor = LoadCursor(
                IntPtr.Zero, new IntPtr((int) Cursor.IDC_ARROW));
            var wndClassEx = new WNDCLASSEX
            {
                cbSize = Marshal.SizeOf<WNDCLASSEX>(),
                style = 0,
                lpfnWndProc = wndProcDelegate,
                hInstance = GetModuleHandle(null),
                hCursor = defaultCursor,
                hbrBackground = IntPtr.Zero,
                lpszClassName = "WalterlvSplashWindow"
            };
            var atom = RegisterClassEx(ref wndClassEx);
            var hwnd = CreateWindowEx(
                0,
                atom,
                null,
                (int) (WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_EX_DLGMODALFRAME),
                CW_USEDEFAULT,
                CW_USEDEFAULT,
                CW_USEDEFAULT,
                CW_USEDEFAULT,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);
            ShowWindow(hwnd, ShowWindowCommand.Restore);
        }

        public void Loop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                GetMessage(out var msg, IntPtr.Zero, 0, 0);
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
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
            return DefWindowProc(hWnd, msg, wParam, lParam);
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
