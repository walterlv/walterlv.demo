using Cvte.Cli;

namespace Walterlv
{
    [Verb("Edit")]
    public class EditOptions
    {
        [Value(0), Option('f', "File")] public string FilePath { get; set; }
    }

    [Verb("Print")]
    public class PrintOptions
    {
        [Value(0), Option('f', "File")] public string FilePath { get; set; }
        [Option('p', "Printer")] public string Printer { get; set; }
    }

    [Verb("Share")]
    public class ShareOptions
    {
        [Option('t', "Target")] public string Target { get; set; }
    }

    public class SelfWrittenEditOptionsParser : CommandLineOptionParser<EditOptions>
    {
        public SelfWrittenEditOptionsParser()
        {
            var options = new EditOptions();
            Verb = "Edit";
            AddMatch(0, value => options.FilePath = value);
            AddMatch('f', value => options.FilePath = value);
            AddMatch("File", value => options.FilePath = value);
            SetResult(() => options);
        }
    }

    public class SelfWrittenPrintOptionsParser : CommandLineOptionParser<PrintOptions>
    {
        public SelfWrittenPrintOptionsParser()
        {
            var options = new PrintOptions();
            Verb = "Print";
            AddMatch(0, value => options.FilePath = value);
            AddMatch('f', value => options.FilePath = value);
            AddMatch("File", value => options.FilePath = value);
            AddMatch('p', value => options.Printer = value);
            AddMatch("Printer", value => options.Printer = value);
            SetResult(() => options);
        }
    }

    public class SelfWrittenShareOptionsParser : CommandLineOptionParser<ShareOptions>
    {
        public SelfWrittenShareOptionsParser()
        {
            var options = new ShareOptions();
            Verb = "Share";
            AddMatch('t', value => options.Target = value);
            AddMatch("Target", value => options.Target = value);
            SetResult(() => options);
        }
    }
}