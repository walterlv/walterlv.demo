using System;
using System.Collections.Generic;

namespace Walterlv.Framework
{
    public class CommandLine
    {
        private readonly Dictionary<string, List<string>> _optionArgs = new Dictionary<string, List<string>>();

        public static CommandLine Parse(string[] args)
        {
            var optionArgs = new Dictionary<string, List<string>>();

            var cmd = new CommandLine();

            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    // 长名称标识符。
                }
                else if (arg.StartsWith("-"))
                {
                    // 短名称标识符。
                }
                else
                {
                    // 值。
                }
            }

            return cmd;

            void Append(string name, string value)
            {

            }
        }

        public T As<T>()
        {
            return default;
        }
    }
}