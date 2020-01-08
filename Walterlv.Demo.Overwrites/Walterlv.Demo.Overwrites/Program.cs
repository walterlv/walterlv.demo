using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Walterlv.Demo.Overwrites
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.md");
                PrepareFile(filePath);
                await WriteShorterFileAsync(filePath).ConfigureAwait(false);
                VerifyContents(filePath);
                Console.ReadLine();
            }
        }

        private static void PrepareFile(string filePath)
        {
            File.WriteAllText(filePath, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }

        private static async Task WriteShorterFileAsync(string filePath)
        {
            using var fileStream = File.OpenWrite(filePath);
            fileStream.SetLength(0);
            using var stream = new StreamWriter(fileStream, Encoding.UTF8);
            await stream.WriteAsync("oooooooo").ConfigureAwait(false);
        }

        private static void VerifyContents(string filePath)
        {
            var text = File.ReadAllText(filePath, Encoding.UTF8);
            Console.WriteLine(text);
        }
    }
}
