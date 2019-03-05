using System;
using Walterlv.Framework;

namespace Walterlv
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = CommandLine.Parse(args, urlProtocol: "walterlv");
            var option = commandLine.As<Option>(new OptionParser());

            Console.ReadLine();
        }
    }

    internal enum StartupMode
    {
        Edit,
        Display,
    }
}
