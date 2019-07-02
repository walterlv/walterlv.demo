using System;
using System.IO;

namespace Walterlv.Demo.CatchWhenCrash
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                try
                {
                    Console.WriteLine("Try");
                    throw new FileNotFoundException();
                }
                catch (FileNotFoundException ex) when (ex.FileName.EndsWith(".png"))
                {
                    Console.WriteLine("Catch 1");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Catch 2");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Catch 3");
            }
            Console.WriteLine("End");
        }
    }
}
