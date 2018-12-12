using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            helper();
            helper();
            helper();
            helper();
            helper();

            Console.ReadLine();
        }

        static void helper()
        {
            Console.WriteLine("this is a test");
        }
    }
}
