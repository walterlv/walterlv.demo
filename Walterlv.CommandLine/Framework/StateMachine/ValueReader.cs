using System;

namespace Walterlv.Framework.StateMachine
{
    internal class ValueReader : ICommandLineArgReader
    {
        public bool Match(string arg) => !arg.StartsWith("-", StringComparison.CurrentCultureIgnoreCase);

        public void Read(ICommandLineStateMachine reader, string arg)
        {
            reader.AppendValue(arg);
        }
    }
}