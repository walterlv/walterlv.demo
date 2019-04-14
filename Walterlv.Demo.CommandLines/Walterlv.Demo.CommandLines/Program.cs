using System;
using System.Globalization;

namespace Walterlv.Demo.CommandLines
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main 函数参数列表中参数总数：{args.Length}");
            OutputArgsInfo(args);

            args = Environment.GetCommandLineArgs();
            Console.WriteLine($"GetCommandLineArgs 参数总数：{args.Length}");
            OutputArgsInfo(args);

            Console.WriteLine($"按任意键继续……");
            Console.ReadKey();
        }

        private static void OutputArgsInfo(string[] args)
        {
            var digitCount = (args.Length - 1).ToString(CultureInfo.InvariantCulture).Length;

            for (var i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"[{i.ToString().PadLeft(digitCount, ' ')}] {args[i]}");
            }
        }
    }
}
