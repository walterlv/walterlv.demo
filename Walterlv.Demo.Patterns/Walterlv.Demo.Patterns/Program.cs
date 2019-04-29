using System;

namespace Walterlv.Demo.Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Fantastic fantastic = (IFantastic) null;
            Console.WriteLine(fantastic);
        }
    }
}