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

        [Benchmark]
        public void ParseAsOptions()
        {
            var commandLine = Framework.CommandLine.Parse(CommandLineArguments, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }
    }
}
