using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using Walterlv.Demo.Windows;

namespace Walterlv.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SourceInitialized += MainWindow_SourceInitialized;
            Loaded += MainWindow_Loaded;

            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Thread.Sleep(50);
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            var childWindow = new ChildWindow();
            var helper = new WindowInteropHelper(childWindow);
            var wrapper = new WindowWrapper(helper.EnsureHandle())
            {
                Margin = new Thickness(16),
            };
            Grid.SetRow(wrapper, 1);
            Grid.SetColumnSpan(wrapper, 3);
            RootPanel.Children.Add(wrapper);

            //var source = (HwndSource)PresentationSource.FromVisual(this);
            //source.AddHook(OnWndProc);

            //const int CS_VREDRAW = 0x0001;
            //const int CS_HREDRAW = 0x0002;
            //var styles = Lsj.Util.Win32.User32.GetWindowLong(source.Handle, Lsj.Util.Win32.Enums.GetWindowLongIndexes.GWL_EXSTYLE);
            //styles = new IntPtr(styles.ToInt32() | CS_HREDRAW | CS_VREDRAW);
            //Lsj.Util.Win32.User32.SetWindowLong(source.Handle, Lsj.Util.Win32.Enums.GetWindowLongIndexes.GWL_EXSTYLE, styles);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //WindowAccentCompositor.DisableDwm(new WindowInteropHelper(this).Handle);
        }

        private IntPtr OnWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_ERASEBKGND = 0x0014;
            const int WM_NCCALCSIZE = 0x0083;
            const int WVR_HREDRAW = 0x0100;
            const int WVR_VREDRAW = 0x0200;
            switch (msg)
            {
                case WM_ERASEBKGND:
                    return new IntPtr(1);
                case WM_NCCALCSIZE:
                    //ResizeWindows(0,0,(int)ActualWidth,(int)ActualHeight);
                    //handled = true;
                    break;
                    //return new IntPtr(WVR_HREDRAW | WVR_VREDRAW);
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        private void ResizeWindows(int x, int y, int width, int height)
        {
            var helper = new WindowInteropHelper(this);
            var handle = helper.Handle;
            var relationship = WindowWrapper.GetRelationship(handle);
            var hdwp = BeginDeferWindowPos(relationship.Count);
            foreach (var pair in relationship)
            {
                pair.Value.ArrangeChild(new Size(width - 32, height - 32 - 100));
            }
            EndDeferWindowPos(hdwp);
        }

        [DllImport("user32.dll")]
        static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll")]
        static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);
    }
}
