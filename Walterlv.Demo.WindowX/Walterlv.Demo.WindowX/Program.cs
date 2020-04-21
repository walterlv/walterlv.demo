using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Walterlv.Demo.WindowX
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var last = IntPtr.Zero;
            while (true)
            {
                var current = GetForegroundWindow();
                if (current != last)
                {
                    last = current;
                    var (hWnd, className, title) = GetWindowInfo(current);
                    Console.WriteLine($"[{DateTime.Now:mm:ss}] [{hWnd.ToInt64():X8}] {title} - {className}");
                }
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        private static (IntPtr hWnd, string className, string title) GetWindowInfo(IntPtr hWnd)
        {
            var lptrString = new StringBuilder(512);
            var lpString = new StringBuilder(512);
            GetClassName(hWnd, lpString, lpString.Capacity);
            GetWindowText(hWnd, lptrString, lptrString.Capacity);
            string className = lpString.ToString().Trim();
            string title = lptrString.ToString().Trim();
            return (hWnd, className, title);
        }

        /// <summary>
        ///     Retrieves a handle to the foreground window (the window with which the user is currently working). The system
        ///     assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        ///     <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633505%28v=vs.85%29.aspx for more information.</para>
        /// </summary>
        /// <returns>
        ///     C++ ( Type: Type: HWND )<br /> The return value is a handle to the foreground window. The foreground window
        ///     can be NULL in certain circumstances, such as when a window is losing activation.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lptrString, int nMaxCount);
    }
}
