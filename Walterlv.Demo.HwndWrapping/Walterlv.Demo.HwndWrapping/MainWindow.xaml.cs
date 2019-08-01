using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Walterlv.Demo.HwndWrapping
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class HwndWrapper : HwndHost
    {
        private HwndSource _source;

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            const int WS_CHILD = 0x40000000;
            var owner = ((HwndSource)PresentationSource.FromVisual(this)).Handle;

            var parameters = new HwndSourceParameters("demo")
            {
                ParentWindow = owner,
                WindowStyle = (int)(WS_CHILD),
            };
            _source = new HwndSource(parameters);
            _source.RootVisual = new ChildPage();
            return new HandleRef(this, _source.Handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            _source?.Dispose();
        }
    }
}
