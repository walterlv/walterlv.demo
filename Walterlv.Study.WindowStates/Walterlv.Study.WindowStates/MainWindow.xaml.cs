using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace Walterlv.Study.WindowStates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StateChanged += OnStateChanged;
        }

        private async void OnStateChanged(object sender, EventArgs e)
        {
            // 当 ResizeMode 允许最小化的时候，Win+D 才会进来这里。
            if (WindowState == WindowState.Minimized)
            {
                await Dispatcher.Yield();
                //await Task.Delay(1000);
                WindowState = WindowState.Maximized;
                Activate();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = (HwndSource)HwndSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                if (msg == WmSize)
                {
                    var wparam = wParam.ToInt32();

                    switch (wparam)
                    {
                        case SizeRestored:
                        case SizeMinimized:
                        case SizeMaximized:
                        case SizeShow:
                        case SizeHide:

                            break;
                        default:
                            return IntPtr.Zero;
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception)
            {
                listMessages.Items.Add("err");
                base.WndProc(ref m);
            }
        }
    }
}
