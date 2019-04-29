using System;
using System.Globalization;

namespace Walterlv.Demo.CommandLines
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"参数总数：{args.Length}");

            var digitCount = (args.Length - 1).ToString(CultureInfo.InvariantCulture).Length;

            for (var i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"[{i.ToString().PadLeft(digitCount, ' ')}] {args[i]}");
            }

            Console.WriteLine($"按任意键继续……");
            Console.ReadKey();
        }
    }
}
