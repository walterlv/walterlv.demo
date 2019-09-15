using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Walterlv.HighPerformanceMemory
{
    class Program
    {
        public static Window CurrentWindow { get; set; }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("基准测试开始");
            BenchmarkRunner.Run<MemoryCopyTests>();
            Console.WriteLine("基准测试结束");
        }
    }
}
