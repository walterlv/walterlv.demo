namespace Walterlv.Framework
{
    public sealed class OptionAttribute : CommandLineAttribute
    {
        public char? ShortName { get; }
        public string LongName { get; }

        public OptionAttribute(string longName)
        {
            ShortName = null;
            LongName = longName;
        }

        public OptionAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }
    }
}