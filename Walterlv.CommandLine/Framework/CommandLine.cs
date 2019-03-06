using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using Walterlv.Framework.StateMachine;

namespace Walterlv.Framework
{
    [DebuggerDisplay("CommandLine: {DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(CommandLineDebugView))]
    public class CommandLine
    {
        private readonly Dictionary<string, IReadOnlyList<string>> _optionArgs;

        private CommandLine(Dictionary<string, IReadOnlyList<string>> optionArgs)
            => _optionArgs = optionArgs ?? throw new ArgumentNullException(nameof(optionArgs));

        public T As<T>()
        {
            var optionType = typeof(T);
            var parserType = optionType.Assembly.GetType($"{optionType.FullName}Parser", false, false);
            if (parserType == null)
            {
                throw new NotSupportedException("暂不支持在运行时根据特性解析命令行参数。");
            }

            var parser = (ICommandLineOptionParser<T>) Activator.CreateInstance(parserType);
            return As(parser);
        }

        /// <summary>
        /// 使用指定的命令行参数解析器 <paramref name="parser"/> 解析出参数 <typeparamref name="T"/> 的一个新实例。
        /// </summary>
        /// <typeparam name="T">解析出来的参数实例。</typeparam>
        /// <param name="parser">用于解析 <typeparamref name="T"/> 的解析器实例，此类型通常会在编译期间自动生成。</param>
        /// <returns>命令行参数的新实例。</returns>
        public T As<T>(ICommandLineOptionParser<T> parser)
        {
            foreach (var optionValue in _optionArgs)
            {
                var option = optionValue.Key;
                var values = optionValue.Value;
                if (string.IsNullOrWhiteSpace(option))
                {
                    // 没有选项，只有值。
                    // 包括此 if 分支之外的任何情况下，值都需要保持传入时的大小写。
                    for (var i = 0; i < values.Count; i++)
                    {
                        var value = values[i];
                        parser.SetValue(i, value);
                    }
                }
                else if (option.Length == 2 && option[0] == '-')
                {
                    // 短名称。
                    // 短名称的种类：
                    //  -n
                    //  -N
                    // 短名称是大小写敏感的，不同大小写表示不同的含义。
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
                else if (option.Length > 2 && option[0] == '-')
                {
                    // 长名称。
                    // 长名称的种类：
                    //  -LongName
                    //  --long-name
                    // 以上示例的两种是等价的，但无论哪种，都至少需要三个字符。
                    // 长名称是大小写敏感的，大小写不同的参数将不会被识别。

                    // 格式化长名称，如果不是 -LongName 型，就都转换成 -LongName 型。
                    if (option[1] == '-')
                    {
                        // --long-name 型
                        option = FormatCoreLongName(option);
                    }

                    var longName = option.Substring(1);

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
                    throw new NotSupportedException($"不支持命令行选项：{option}。");
                }
            }

            return parser.Commit();
        }

        public static CommandLine Parse(string[] args, string urlProtocol = null)
        {
            if (!string.IsNullOrWhiteSpace(urlProtocol) && args.Length > 0 && args[0].StartsWith($"{urlProtocol}://"))
            {
                // 如果传入的参数是协议参数，那么进行协议参数解析，并转换成命令行参数风格。
                // 由于 URL 解析不是主流程，所以这里暂时不考虑性能问题。
                args = ConvertUrlToArgs(args[0]);
            }

            var stateMachine = new CommandLineStateMachine(args);
            var parsedArgs = stateMachine.Run();
            return new CommandLine(parsedArgs);
        }

        /// <summary>
        /// 将 URL 转换成符合命令行格式的参数列表。
        /// </summary>
        /// <param name="url">来源于 Web 的 URL。</param>
        /// <returns>符合命令行格式的参数列表。</returns>
        private static string[] ConvertUrlToArgs(string url)
        {
            url = HttpUtility.UrlDecode(url);
            var start = url?.IndexOf('?') ?? -1;
            if (start >= 0 && url != null)
            {
                var arguments = url.Substring(start + 1);
                var args = from keyValueString in arguments.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)
                    let keyValue = keyValueString.Split(new[] {'='})
                    select new[] {FormatShellLongName(keyValue[0]), keyValue[1]};
                return args.SelectMany(x => x).ToArray();
            }

            return new string[0];
        }

        /// <summary>
        /// 将 longName 这种名称转换为 -LongName 这种名称。
        /// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string FormatShellLongName(string option)
        {
            var chars = option.ToCharArray();
            if (char.IsUpper(chars[0]))
            {
                return $"-{option}";
            }

            chars[0] = char.ToUpper(chars[0], CultureInfo.InvariantCulture);
            return $"-{new string(chars)}";
        }

        /// <summary>
        /// 将 --long-name 这种名称转换为 -LongName 这种名称。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string FormatCoreLongName(string option)
        {
            var builder = new StringBuilder();
            builder.Append('-');
            var isWordFirstLetter = true;
            foreach (var current in option.Skip(2))
            {
                if (current is '-')
                {
                    isWordFirstLetter = true;
                }
                else if (isWordFirstLetter)
                {
                    isWordFirstLetter = false;
                    builder.Append(char.ToUpper(current, CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.Append(current);
                }
            }

            return builder.ToString();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Join(" ", _optionArgs
            .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(" ", pair.Value)}"));

        private class CommandLineDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly CommandLine _owner;

            public CommandLineDebugView(CommandLine owner) => _owner = owner;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private string[] Options => _owner._optionArgs
                .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(" ", pair.Value)}")
                .ToArray();
        }
    }
}