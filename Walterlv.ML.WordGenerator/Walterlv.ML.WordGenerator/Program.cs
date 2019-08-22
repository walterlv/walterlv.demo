using dotnetCampus.Cli;
using System;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            await CommandLine.Parse(args)
                .AddHandler<GenerateWordOptions>(o => o.RunAsync())
                .RunAsync();
        }
    }
}
