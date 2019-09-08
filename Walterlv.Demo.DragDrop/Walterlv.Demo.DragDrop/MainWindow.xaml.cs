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

            DragSourceBorder.AllowDrop = true;
            DragSourceBorder.MouseLeave += DragSourceBorder_MouseLeave;

            DropTargetBorder.AllowDrop = true;
        }

        private void DragSourceBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            DragSourceBorder.DragEnter += DragSourceBorder_DragEnter;
            DragSourceBorder.DragLeave += DragSourceBorder_DragLeave;
            DragSourceBorder.QueryContinueDrag += DragSourceBorder_QueryContinueDrag;
            DragDrop.DoDragDrop(DragSourceBorder, "数据", DragDropEffects.Move);
            DragSourceBorder.DragEnter -= DragSourceBorder_DragEnter;
            DragSourceBorder.DragLeave -= DragSourceBorder_DragLeave;
            DragSourceBorder.QueryContinueDrag -= DragSourceBorder_QueryContinueDrag;
        }

        private bool _isDraggingIn = false;

        private void DragSourceBorder_DragEnter(object sender, DragEventArgs e)
        {
            _isDraggingIn = true;
        }

        private void DragSourceBorder_DragLeave(object sender, DragEventArgs e)
        {
            _isDraggingIn = false;
        }

        private void DragSourceBorder_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (_isDraggingIn)
            {
                e.Action = DragAction.Cancel;
            }
        }

        [DllImport("dwmapi.dll")]
        static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        [StructLayout(LayoutKind.Sequential)]
        internal struct PSIZE
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public Rect rcDestination;
            public Rect rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            internal Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }

    public class HwndWrapper : HwndHost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("代码质量", "IDE0067:丢失范围之前释放对象", Justification = "子窗口生命周期与主窗口一致，无需回收。")]
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            const int WS_CHILD = 0x40000000;
            const int WS_CLIPCHILDREN = 0x02000000;
            var source = new HwndSource(new HwndSourceParameters
            {
                ParentWindow = ((HwndSource)PresentationSource.FromVisual(this)).Handle,
                WindowStyle = WS_CHILD | WS_CLIPCHILDREN,
            });
            return new HandleRef(this, source.Handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
        }
    }
}
