using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Cvte.Windows.Native;
using static Cvte.Windows.Native.Win32.WindowStyles;

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

        protected override Size ArrangeOverride(Size finalSize)
        {
            Win32.User32.GetWindowRect(_source.Handle, out var bounds);
            return new Size(bounds.Width, bounds.Height);
        }
    }
}
