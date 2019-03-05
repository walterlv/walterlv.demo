using System.Collections.Generic;

namespace Walterlv.Framework
{
    public interface ICommandLineOptionParser<out T>
    {
        void SetValue(int index, string value);
        void SetValue(char shortName, bool value);
        void SetValue(char shortName, string value);
        void SetValue(char shortName, IEnumerable<string> values);
        void SetValue(string longName, bool value);
        void SetValue(string longName, string value);
        void SetValue(string longName, IEnumerable<string> values);
        T Commit();
    }
}