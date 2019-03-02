using System;
using Walterlv.Framework;

namespace Walterlv
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = CommandLine.Parse(args);

            Console.ReadLine();
        }
    }

    internal enum StartupMode
    {
        Edit,
        Display,
    }
}
