using System;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Demo.StaticConstructor
{
    class Program
    {
        private static readonly ManualResetEventSlim _locker = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            Parallel.For(0, 8, i =>
            {
                if (i < 4)
                {
                    Read(i);
                }
                else
                {
                    Write(i);
                }
            });
        }

        private static void Read(int i)
        {
            _locker.Wait();

            Console.WriteLine($"Read S {i}");
            Console.WriteLine($"Read E {i}");
        }

        private static void Write(int i)
        {
            _locker.Reset();

            Task.Run(() =>
            {
                Console.WriteLine($"Write S {i}");
                Thread.Sleep(1000);
                Console.WriteLine($"Write E {i}");

                _locker.Set();
            });
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Parallel.For(0, 16, i =>
    //        {
    //            Console.WriteLine($"[{i.ToString().PadLeft(2)}] {Foo.Value}");
    //        });
    //    }
    //}

    //public class Foo
    //{
    //    public static int Value = 0;

    //    static Foo()
    //    {
    //        Thread.Sleep(3000);
    //        Value++;
    //    }
    //}
}
