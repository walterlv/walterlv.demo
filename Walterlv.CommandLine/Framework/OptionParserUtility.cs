using System;
using System.Collections.Generic;
using System.Linq;

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
                    for (var i = 0; i < values.Count; i++)
                    {
                        var value = values[i];
                        parser.SetValue(i, value);
                    }
                }
                else if (option.Length == 2 && option[0] == '-')
                {
                    // 短名称。
                    var shortName = option[1];
                    if (values.Count == 0)
                    {
                        parser.SetValue(shortName, true);
                    }
                    else if (values.Count == 1)
                    {
                        if (bool.TryParse(values[0], out var @bool))
                        {
                            parser.SetValue(shortName, @bool);
                        }
                        else
                        {
                            parser.SetValue(shortName, values[0]);
                        }
                    }
                    else
                    {
                        parser.SetValue(shortName, values);
                    }
                }
                else if (option.Length > 2 && option[0] == '-' && option[1] == '-')
                {
                    // 长名称。
                    var longName = option.Substring(2);
                    if (values.Count == 0)
                    {
                        parser.SetValue(longName, true);
                    }
                    else if (values.Count == 1)
                    {
                        if (bool.TryParse(values[0], out var @bool))
                        {
                            parser.SetValue(longName, @bool);
                        }
                        else
                        {
                            parser.SetValue(longName, values[0]);
                        }
                    }
                    else
                    {
                        parser.SetValue(longName, values);
                    }
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