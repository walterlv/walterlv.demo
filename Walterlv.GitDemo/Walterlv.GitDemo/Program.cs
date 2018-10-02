using System;
using System.Globalization;

namespace Walterlv.GitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var titleCase = new CultureInfo("en-us").TextInfo.ToTitleCase("knowledge management is on the way");
            Console.WriteLine(titleCase);

            Console.WriteLine("walterlv 的自动 git 命令");

            var git = new CommandRunner("git", @"D:\Developments\Blogs\walterlv.github.io");
            var status = git.Run("status");

            Console.WriteLine(status);
            Console.WriteLine("按 Enter 退出程序……");
            Console.ReadLine();
        }
    }
}