using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Walterlv.Demo.HighPerformanceBitmap
{
    public partial class MainWindow : Window
    {
        private readonly WriteableBitmap _bitmap;

        public MainWindow()
        {
            InitializeComponent();

            _bitmap = new WriteableBitmap(3840, 2160, 96.0, 96.0, PixelFormats.Pbgra32, null);
            Image.Source = _bitmap;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private readonly byte[] _empty4KBitmapArray = new byte[3840 * 2160 * 4];

        private unsafe void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;

            _bitmap.Lock();

            //fixed (byte* ptr = _empty4KBitmapArray)
            //{
            //    CopyMemory(_bitmap.BackBuffer, new IntPtr(ptr), (uint)_empty4KBitmapArray.Length);
            //}

            fixed (byte* ptr = _empty4KBitmapArray)
            {
                var p = new IntPtr(ptr);
                Buffer.MemoryCopy(ptr, _bitmap.BackBuffer.ToPointer(), _empty4KBitmapArray.Length, _empty4KBitmapArray.Length);
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, 2560, 1440));
            _bitmap.Unlock();
        }

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
    }
}
