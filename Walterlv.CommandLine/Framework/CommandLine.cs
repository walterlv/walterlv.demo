using System;
using System.Collections.Generic;

namespace Walterlv.Framework
{
    public class CommandLine
    {
        private readonly Dictionary<string, List<string>> _optionArgs = new Dictionary<string, List<string>>();

        public static CommandLine Parse(string[] args, string urlProtocol = null)
        {
            var optionArgs = new Dictionary<string, List<string>>();

            var cmd = new CommandLine();

            string currentOption;
            foreach (var arg in args)
            {
                // 可判断的参数类型有：
                // value            表示一个按参数位置确定的值可选参数，必须出现在可选参数之前。
                // -x               表示一个布尔值可选参数。
                // -x value         表示带有一个参数的可选参数。
                // -x value0 value1 表示带有多个参数的可选参数。
                if (arg.StartsWith("-"))
                {
                    // 名称标识符。
                    // 虽然这里可以根据 `-` 以及 `--` 来区分长名称与短名称，但因为不知道长名称与短名称的对应关系，所以无法将其关联。
                    // 需要等到参数类型确认后才能知道长短名称的对应关系。
                }
                else
                {
                    // 值。
                }
            }

            return cmd;

            void Append(string name, string value)
            {
                if (!optionArgs.TryGetValue(name, out var list))
                {
                    list = new List<string>();
                    optionArgs[name] = list;
                }

                list.Add(value);
            }
        }

        public T As<T>()
        {
            return default;
        }
    }
}