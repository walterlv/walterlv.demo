using System;

namespace Walterlv.Demo.LOH
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new byte[1024 * 1024 * 1024];
            Console.WriteLine(x);
            for (var i = 0; i < 10; i++)
            {
                x = new byte[1024 * 1024 * 1024];
                Console.WriteLine(x);
            }
            GC.Collect();
            GC.WaitForFullGCComplete();
            Console.ReadLine();
        }
    }
}
