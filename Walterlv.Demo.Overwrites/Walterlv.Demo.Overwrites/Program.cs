using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Demo.Overwrites
{
    class Program
    {
        static async Task Main(string[] args)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                Console.Write($"[{i.ToString().PadLeft(5, ' ')}]");
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.md");
                PrepareFile(filePath);
                await WriteShorterFileAsync(filePath).ConfigureAwait(false);
                VerifyContents(filePath);
                //Console.ReadLine();
            }
        }

        private static void PrepareFile(string filePath)
        {
            File.WriteAllText(filePath, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }

        private static async Task WriteShorterFileAsync(string filePath)
        {
            using var fileStream = File.OpenWrite(filePath);
            //using var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            fileStream.SetLength(0);
            //using var a = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            //var reader = new StreamReader(a);
            using var stream = new StreamWriter(fileStream, Encoding.UTF8);
            await stream.WriteAsync("oooooooo").ConfigureAwait(false);
        }

        private static void VerifyContents(string filePath)
        {
            var text = File.ReadAllText(filePath, Encoding.UTF8);
            if (text.Length != 8)
            {
                Console.WriteLine("长度错误");
                Console.ReadLine();
            }
            Console.WriteLine(text);
        }
    }
}
