using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Walterlv.Demo.MkLink
{
    class Program
    {
        static void Main(string[] args)
        {
            JunctionPoint.Create("walterlv.demo", @"D:\Developments", true);
        }
    }
}
