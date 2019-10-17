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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            MouseMove += MainWindow_MouseMove;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(Image);
            var x = (int)(position.X / Image.ActualWidth * _bitmap.PixelWidth);
            var y = (int)(position.Y / Image.ActualHeight * _bitmap.PixelHeight);
            _mousePosition = (x, y);
        }

        private (int x, int y) _mousePosition;
        private readonly byte[] _empty4KBitmapArray = new byte[3840 * 2160 * 4];

        private unsafe void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            var (x, y) = _mousePosition;

            _bitmap.Lock();

            // 此方法使用内存拷贝，性能好于下面遍历（7% CPU）。
            fixed (byte* ptr = _empty4KBitmapArray)
            {
                Buffer.MemoryCopy(ptr, _bitmap.BackBuffer.ToPointer(), _empty4KBitmapArray.Length, _empty4KBitmapArray.Length);
            }

            // 此方法使用遍历写入，性能略差（18% CPU）。
            //var buffer = (byte*)_bitmap.BackBuffer.ToPointer();
            //for (int j = 0; j < height; j++)
            //{
            //    for (var i = 0; i < width; i++)
            //    {
            //        if (i > x - 4 && i < x + 4 && j > y - 4 && j < y + 4)
            //        {
            //            var pixel = buffer + j * width * 4 + i * 4;
            //            *pixel = 0xff;
            //            *(pixel + 1) = 0x7f;
            //            *(pixel + 2) = 0x00;
            //            *(pixel + 3) = 0xff;
            //        }
            //        else
            //        {

            //        }
            //    }
            //}

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, 100, 100));
            _bitmap.Unlock();
        }
    }
}
