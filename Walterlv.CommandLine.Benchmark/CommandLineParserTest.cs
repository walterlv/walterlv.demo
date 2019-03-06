using BenchmarkDotNet.Attributes;
using CommandLine;

namespace Walterlv.CommandLine.Benchmark
{
    public class CommandLineParserTest
    {
        private static readonly string[] NoArgs = new string[0];

        private static readonly string[] WindowsStyleArgs =
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

        private static readonly string[] LinuxStyleArgs =
        {
            @"C:\Users\lvyi\Desktop\重命名试验.enbx",
            "--cloud",
            "--iwb",
            "-m",
            "Display",
            "-s",
            "-p",
            "Outside",
            "--startup-session",
            "89EA9D26-6464-4E71-BD04-AA6516063D83",
        };

        private static readonly string[] UrlArgs =
        {
            @"walterlv://open/?file=C:\Users\lvyi\Desktop\%E9%87%8D%E5%91%BD%E5%90%8D%E8%AF%95%E9%AA%8C.enbx&cloud=true&iwb=true&silence=true&placement=Outside&startupSession=89EA9D26-6464-4E71-BD04-AA6516063D83",
        };

        [Benchmark]
        public void ParseNoArgs()
        {
            var commandLine = Framework.CommandLine.Parse(NoArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseNoArgsAuto()
        {
            var commandLine = Framework.CommandLine.Parse(NoArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark(Baseline = true)]
        public void ParseWindows()
        {
            var commandLine = Framework.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseWindowsAuto()
        {
            var commandLine = Framework.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark]
        public void ParseLinux()
        {
            var commandLine = Framework.CommandLine.Parse(LinuxStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseLinuxAuto()
        {
            var commandLine = Framework.CommandLine.Parse(LinuxStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark]
        public void ParseUrl()
        {
            var commandLine = Framework.CommandLine.Parse(UrlArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        [Benchmark]
        public void ParseUrlAuto()
        {
            var commandLine = Framework.CommandLine.Parse(UrlArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark]
        public void CommandLineParser()
        {
            Parser.Default.ParseArguments<CommandLineOptions>(LinuxStyleArgs).WithParsed(options => { });
        }
    }
}