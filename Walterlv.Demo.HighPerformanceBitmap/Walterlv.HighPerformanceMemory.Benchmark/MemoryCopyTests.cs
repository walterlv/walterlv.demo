using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace Walterlv.HighPerformanceMemory
{
    public class MemoryCopyTests
    {
        private readonly byte[] _empty4KBitmapArray = new byte[3840 * 2160 * 4];
        private readonly WriteableBitmap _bitmap = new WriteableBitmap(3840, 2160, 96.0, 96.0, PixelFormats.Pbgra32, null);

        [Benchmark(Description = "CopyMemory")]
        [Arguments(3840, 2160)]
        [Arguments(100, 100)]
        public unsafe void CopyMemory(int width, int height)
        {
            _bitmap.Lock();

            fixed (byte* ptr = _empty4KBitmapArray)
            {
                var p = new IntPtr(ptr);
                CopyMemory(_bitmap.BackBuffer, new IntPtr(ptr), (uint)_empty4KBitmapArray.Length);
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            _bitmap.Unlock();
        }

        [Benchmark(Description = "RtlMoveMemory")]
        [Arguments(3840, 2160)]
        [Arguments(100, 100)]
        public unsafe void RtlMoveMemory(int width, int height)
        {
            _bitmap.Lock();

            fixed (byte* ptr = _empty4KBitmapArray)
            {
                var p = new IntPtr(ptr);
                MoveMemory(_bitmap.BackBuffer, new IntPtr(ptr), (uint)_empty4KBitmapArray.Length);
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            _bitmap.Unlock();
        }

        [Benchmark(Baseline = true, Description = "Buffer.MemoryCopy")]
        [Arguments(3840, 2160)]
        [Arguments(100, 100)]
        public unsafe void BufferMemoryCopy(int width, int height)
        {
            _bitmap.Lock();

            fixed (byte* ptr = _empty4KBitmapArray)
            {
                var p = new IntPtr(ptr);
                Buffer.MemoryCopy(ptr, _bitmap.BackBuffer.ToPointer(), _empty4KBitmapArray.Length, _empty4KBitmapArray.Length);
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            _bitmap.Unlock();
        }

        [Benchmark(Description = "for for")]
        [Arguments(3840, 2160)]
        [Arguments(100, 100)]
        public unsafe void ForForCopy(int width, int height)
        {
            _bitmap.Lock();

            var buffer = (byte*)_bitmap.BackBuffer.ToPointer();
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var pixel = buffer + j * width * 4 + i * 4;
                    *pixel = 0xff;
                    *(pixel + 1) = 0x7f;
                    *(pixel + 2) = 0x00;
                    *(pixel + 3) = 0xff;
                }
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            _bitmap.Unlock();
        }

        [DllImport("kernel32.dll")]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);
    }
}
