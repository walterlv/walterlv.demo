using dotnetCampus.Cli;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CommandLine.Parse(args)
                .AddHandler<ParseSolutionCodeOptions>(o => o.RunAsync())
                .RunAsync();
        }
    }
}
