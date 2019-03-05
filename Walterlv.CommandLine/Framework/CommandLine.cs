using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        {
            _optionArgs = optionArgs ?? throw new ArgumentNullException(nameof(optionArgs));
        }

        public T As<T>()
        {
            return default;
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
                    let key = $"--{keyValue[0]}"
                    let value = keyValue[1]
                    select new[] {key, value};
                return args.SelectMany(x => x).ToArray();
            }

            return new string[0];
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Join(' ', _optionArgs
            .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(' ', pair.Value)}"));

        private class CommandLineDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly CommandLine _owner;

            public CommandLineDebugView(CommandLine owner) => _owner = owner;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private string[] Options => _owner._optionArgs
                .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(' ', pair.Value)}")
                .ToArray();
        }
    }
}