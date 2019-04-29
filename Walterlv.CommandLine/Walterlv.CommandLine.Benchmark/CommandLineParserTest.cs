using BenchmarkDotNet.Attributes;
using CommandLine;
using Cvte.Cli;

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

        internal static readonly string[] CmdStyleArgs =
        {
            @"C:\Users\lvyi\Desktop\重命名试验.enbx",
            "/Cloud",
            "/Iwb",
            "/m",
            "Display",
            "/s",
            "/p",
            "Outside",
            "/StartupSession",
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

        internal static readonly string[] EditVerbArgs =
        {
            "Edit", "XXX",
        };

        //[Benchmark]
        public void ParseNoArgs()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(NoArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        //[Benchmark]
        public void ParseNoArgsAuto()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(NoArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        [Benchmark(Baseline = true)]
        public void ParseWindows()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        //[Benchmark]
        public void ParseWindowsAuto()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        //[Benchmark]
        public void ParseWindowsRuntime()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<RuntimeOptions>();
        }

        //[Benchmark]
        public void ParseWindowsImmutableRuntime()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(WindowsStyleArgs, urlProtocol: "walterlv");
            commandLine.As<RuntimeImmutableOptions>();
        }

        //[Benchmark]
        public void HandleVerbs()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(EditVerbArgs, urlProtocol: "walterlv");
            commandLine.Handle<EditOptions, PrintOptions>(options => 0, options => 0,
                new SelfWrittenEditOptionsParser(), new SelfWrittenPrintOptionsParser());
        }

        //[Benchmark]
        public void HandleVerbsRuntime()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(EditVerbArgs, urlProtocol: "walterlv");
            commandLine.Handle<EditOptions, PrintOptions>(options => 0, options => 0);
        }

        //[Benchmark]
        public void ParseCmd()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(CmdStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        //[Benchmark]
        public void ParseCmdAuto()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(CmdStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        //[Benchmark]
        public void ParseLinux()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(LinuxStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        //[Benchmark]
        public void ParseLinuxAuto()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(LinuxStyleArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        //[Benchmark]
        public void ParseUrl()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(UrlArgs, urlProtocol: "walterlv");
            commandLine.As<Options>(new OptionsParser());
        }

        //[Benchmark]
        public void ParseUrlAuto()
        {
            var commandLine = Cvte.Cli.CommandLine.Parse(UrlArgs, urlProtocol: "walterlv");
            commandLine.As<Options>();
        }

        //[Benchmark]
        public void CommandLineParser()
        {
            Parser.Default.ParseArguments<ComparedOptions>(LinuxStyleArgs).WithParsed(options => { });
        }
    }
}