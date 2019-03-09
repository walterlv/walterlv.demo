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
}