using System;
using Walterlv.Demo.InstallViaMounting.Utils;

namespace Walterlv.Demo.InstallViaMounting
{
    class Program
    {
        static void Main(string[] args)
        {
            var diskpart = new CommandRunner("diskpart", @"C:\Users\lvyi\Desktop\EasiNote5_5.1.12.64091");
            using var cmd = diskpart.Start("");
            cmd.WriteLine("list disk");
        }
    }
}
