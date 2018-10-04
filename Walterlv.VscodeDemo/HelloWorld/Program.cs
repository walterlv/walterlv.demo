using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start("");
            Task.Delay(5000).Wait();
            Process.GetProcessById(0).Kill();
            foreach (var process in Process.GetProcessesByName("easinote"))
            {
                try
                {
                    process.Kill();
                }
                catch (Win32Exception)
                {
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}
