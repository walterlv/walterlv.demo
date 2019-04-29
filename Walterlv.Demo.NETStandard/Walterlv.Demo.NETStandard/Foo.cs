using System;
using System.Runtime.InteropServices;

namespace Walterlv.Demo.NETStandard
{
    public class Foo
    {
        public void Run()
        {
            Console.WriteLine("EntryPoint");
            var intptr = Marshal.StringToCoTaskMemUTF8("e");
            Marshal.ZeroFreeCoTaskMemUTF8(intptr);
            Console.ReadKey();
        }
    }
}
