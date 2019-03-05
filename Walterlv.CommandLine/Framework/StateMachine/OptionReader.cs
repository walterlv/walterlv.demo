using System;

namespace Walterlv.Framework.StateMachine
{
    internal class OptionReader : ICommandLineArgReader
    {
        public bool Match(string arg) => arg.StartsWith("-", StringComparison.CurrentCultureIgnoreCase);

        public void Read(ICommandLineStateMachine reader, string arg)
        {
            reader.Commit();
            reader.SetOption(arg);
        }
    }
}