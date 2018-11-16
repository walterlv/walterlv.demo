using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using static Walterlv.Windows.Native.Win32;

namespace Walterlv.Demo.DpiAwareness
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                var hwnd = ((HwndSource) HwndSource.FromVisual(this)).Handle;
                var dpi = GetDpiForWindow(hwnd);
                MonitorDpiInfoRun.Text = dpi.ToString();
                await Task.Delay(1000);
            }
        }

        public const string LibraryName = "user32.dll";

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern int GetSystemMetricsForDpi(SystemMetrics nIndex, uint dpi);

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool AdjustWindowRectExForDpi([In] [Out] ref Rectangle lpRect,
            bool bMenu, ExtendedWindowStyles dwExStyle, uint dpi);

        [DllImport(LibraryName, CharSet = Properties.BuildCharSet)]
        public static extern bool SystemParametersInfoForDpi(uint uiAction, uint uiParam, IntPtr pvParam,
            SystemParamtersInfoFlags fWinIni, uint dpi);

        [DllImport(LibraryName, CharSet = Properties.BuildCharSet)]
        public static extern uint GetDpiForWindow(IntPtr hwnd);
    }
}
