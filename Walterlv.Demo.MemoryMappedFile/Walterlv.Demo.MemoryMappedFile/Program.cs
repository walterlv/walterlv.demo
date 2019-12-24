using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            for (int i = 0; i < 1000; i++)
            {
                Task.WaitAll(
                    Task.Run(Read1Async),
                    Task.Run(Write1Async)
                    );
                Console.WriteLine();
            }
        }

        private static async Task Read1Async()
        {
            using var token = EnterSafeReadRegion();

            var hasRead = File.ReadAllText("a.md");
            Console.WriteLine($"读出 {hasRead}");
        }

        private static async Task Write1Async()
        {
            using var token = EnterSafeWriteRegion();

            var toWrite = DateTime.Now.Ticks.ToString();
            File.WriteAllText("a.md", toWrite);

            Console.WriteLine($"已写 {toWrite}");
        }

        private static IDisposable EnterSafeReadRegion()
        {
            return SafeReadWriteToken.GetForFile("a.md");
        }

        private static IDisposable EnterSafeWriteRegion()
        {
            return SafeReadWriteToken.GetForFile("a.md");
        }
    }
}
