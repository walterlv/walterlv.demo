using System;
using System.IO;
using System.Threading.Tasks;

namespace Walterlv.DirectXDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var file = new FileInfo(@"C:\Users\lvyi\AppData\Roaming\Seewo\EasiNote5\Data\17417510003\Configs.fkv");
            while (true)
            {
                Console.WriteLine($"{file.Exists} - {File.Exists(file.FullName)}");
                await Task.Delay(1000);
            }
        }
    }
}
