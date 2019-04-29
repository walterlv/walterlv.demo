using System;
using System.IO;

namespace Walterlv.Demo.Finalizers
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {

            };
            Foo foo = null;
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    foo = new Foo();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                try
                {
                    foo = new Foo();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            foo = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();
            Console.ReadLine();
        }
    }

    public class Foo
    {
        public Foo()
        {
            throw new InvalidOperationException("无法创建");
        }

        ~Foo()
        {
            Console.WriteLine("析构");
            throw new InvalidOperationException("无法析构");
        }
    }
}
