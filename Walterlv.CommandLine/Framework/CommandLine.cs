using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                // 如果传入的参数是协议参数，那么进行协议参数解析。
                args = ConvertUrlToArgs(args[0]);
            }

            var stateMachine = new CommandLineStateMachine(args);
            var parsedArgs = stateMachine.Run();
            return new CommandLine(parsedArgs);
        }

        private static string[] ConvertUrlToArgs(string url)
        {
            return new[] {url};
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Join(' ', _optionArgs
            .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(' ', pair.Value)}"));

        private class CommandLineDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly CommandLine _owner;

            public CommandLineDebugView(CommandLine owner)
            {
                _owner = owner;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private string[] Options => _owner._optionArgs
                .Select(pair => $"{pair.Key}{(pair.Key == null ? "" : " ")}{string.Join(' ', pair.Value)}")
                .ToArray();
        }
    }
}