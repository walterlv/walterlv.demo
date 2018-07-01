using System;
using System.Threading;
using Walterlv.Demo.Win32;

namespace Walterlv.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new Win32Window();
            window.Show();
        }
    }
}
