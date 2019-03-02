using System;

namespace Walterlv.Framework
{
    internal readonly struct PrecompiledOptionItem<T>
    {
        internal readonly string ShortName;
        internal readonly string LongName;
        internal readonly Action<T> WhenFound;
        internal readonly Action<T, string> AppendValue;

        internal PrecompiledOptionItem(string shortName, string longName,
            Action<T> whenFound, Action<T, string> appendValue)
        {
            ShortName = shortName;
            LongName = longName;
            WhenFound = whenFound;
            AppendValue = appendValue;
        }
    }
}