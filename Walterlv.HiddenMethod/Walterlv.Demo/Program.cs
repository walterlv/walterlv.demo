using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

//namespace Walterlv.Demo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello World!");
//        }
//    }
//}

class Walterlv
{
    [STAThread]
    static void Main(string[] args)
    {
        Demo.Foo();
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
}
