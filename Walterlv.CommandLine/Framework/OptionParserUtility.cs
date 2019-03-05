using System;
using System.Collections.Generic;

namespace Walterlv.Framework
{
    internal static class OptionParserUtility
    {
        public static T Parse<T>(this ICommandLineOptionParser<T> parser,
            Dictionary<string, IReadOnlyList<string>> options)
        {
            foreach (var optionValue in options)
            {
                var option = optionValue.Key;
                var values = optionValue.Value;
                if (string.IsNullOrWhiteSpace(option))
                {
                    // 没有选项，只有值。
                }
                else if (option.Length == 2 && option[0] == '-')
                {
                    // 短名称。
                    parser[option[1]] = values;
                }
                else if (option.Length > 2 && option[0] == '-' && option[1] == '-')
                {
                    // 长名称。
                }
                else
                {
                    // 参数格式不正确或不支持。
                    throw new NotSupportedException($"{option} option is not supported.");
                }
            }

            return default;
        }
    }
}