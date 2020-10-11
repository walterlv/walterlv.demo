using Lsj.Util.Win32;
using Lsj.Util.Win32.BaseTypes;
using Lsj.Util.Win32.NativeUI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace Walterlv.Win32Demo.DwmTransparencyWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //var window = new Win32Window(typeof(MainWindow).Namespace, "Walterlv.TransparentWindow");
            //window.Show();

            MakeWindowTransparent(new WindowInteropHelper(this).Handle);
        }

        private void MakeWindowTransparent(HWND handle)
        {
            //EnableBlurBehind(handle);
            //ExtendIntoClient(handle);
            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                GlassFrameThickness = new Thickness(-1),
            });
        }

        void EnableBlurBehind(HWND hwnd)
        {
            var bb = new DWM_BLURBEHIND
            {
                dwFlags = DWM_BB.Enable,
                fEnable = true,
                hRgnBlur = IntPtr.Zero,
            };
            DwmEnableBlurBehindWindow(hwnd, ref bb);
        }

        void ExtendIntoClient(HWND hwnd)
        {
            var margin = new MARGINS
            {
                leftWidth = -1,
                rightWidth = -1,
                topHeight = -1,
                bottomHeight = -1,
            };
            DwmExtendFrameIntoClientArea(hwnd, ref margin);
        }

        [DllImport("dwmapi.dll")]
        static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;

            public DWM_BLURBEHIND(bool enabled)
            {
                fEnable = enabled ? true : false;
                hRgnBlur = IntPtr.Zero;
                fTransitionOnMaximized = false;
                dwFlags = DWM_BB.Enable;
            }

            public bool TransitionOnMaximized
            {
                get { return fTransitionOnMaximized; }
                set
                {
                    fTransitionOnMaximized = value ? true : false;
                    dwFlags |= DWM_BB.TransitionMaximized;
                }
            }
        }

        [Flags]
        enum DWM_BB
        {
            Enable = 1,
            BlurRegion = 2,
            TransitionMaximized = 4
        }
    }
}
