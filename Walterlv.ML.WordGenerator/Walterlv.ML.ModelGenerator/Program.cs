using dotnetCampus.Cli;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            foreach (var file in new DirectoryInfo(@"C:\Users\lvyi\AppData\Roaming\Seewo\EasiNote5\Temp\1316-19.08.31,09-33-50,2521578")
                .EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Take(50))
            {
                File.Create(file.DirectoryName + "\\" + Guid.NewGuid().ToString());
            }
            Console.WriteLine();










            await CommandLine.Parse(args)
                .AddHandler<ParseSolutionCodeOptions>(o => o.RunAsync())
                .RunAsync();
        }
    }
}
