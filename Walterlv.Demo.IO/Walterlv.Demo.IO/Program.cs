using System;
using System.IO;

namespace Walterlv.Demo.IO
{
    class Program
    {
        static void Main(string[] args)
        {
            var exists = File.Exists(@"C:\Users\lvyi\Desktop\Walterlv.Demo.IO\Walterlv.Demo.IO");
            var text = File.ReadAllText(@"C:\Users\lvyi\Desktop\Walterlv.Demo.IO\Walterlv.Demo.IO");
            Console.ReadLine();
        }
    }
}
