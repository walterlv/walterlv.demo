using BenchmarkDotNet.Attributes;

namespace Walterlv.CommandLine.Benchmark
{
    public class CommandLineParserTest
    {
        private static readonly string[] CommandLineArguments =
        {
            @"C:\Users\lvyi\Desktop\重命名试验.enbx",
            "-Cloud",
            "-Iwb",
            "-m",
            "Display",
            "-s",
            "-p",
            "Outside",
            "-StartupSession",
            "89EA9D26-6464-4E71-BD04-AA6516063D83",
        };

        private static readonly string[] CommandLineUrl =
        {
            @"walterlv://open/?file=C:\Users\lvyi\Desktop\%E9%87%8D%E5%91%BD%E5%90%8D%E8%AF%95%E9%AA%8C.enbx&cloud=true&iwb=true&silence=true&placement=Outside&startupSession=89EA9D26-6464-4E71-BD04-AA6516063D83",
        };

        [Benchmark(Baseline = true)]
        public void ParseAsOptions()
        {
            var commandLine = Framework.CommandLine.Parse(CommandLineArguments, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseAsAutoOptions()
        {
            var commandLine = Framework.CommandLine.Parse(CommandLineArguments, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark]
        public void ParseUrlAsOptions()
        {
            var commandLine = Framework.CommandLine.Parse(CommandLineUrl, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseUrlAsAutoOptions()
        {
            var commandLine = Framework.CommandLine.Parse(CommandLineUrl, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }
    }
}
