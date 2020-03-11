using System;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Demo.StaticConstructor
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 16, i =>
            {
                Console.WriteLine($"[{i.ToString().PadLeft(2)}] {Foo.Value}");
            });
        }
    }

    public class Foo
    {
        public static int Value = 0;

        static Foo()
        {
            Thread.Sleep(3000);
            Value++;
        }
    }
}
