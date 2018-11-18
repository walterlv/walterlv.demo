using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace Walterlv.Demo.Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "基准性能测试 - 反射 - walterlv";
            BenchmarkRunner.Run<Reflections>();
            if (Debugger.IsAttached) Console.Read();
        }
    }
}
