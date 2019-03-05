using System;
using System.Collections.Generic;
using Walterlv.Framework.StateMachine;

namespace Walterlv.Framework
{
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
            var stateMachine = new CommandLineStateMachine(args);
            var parsedArgs = stateMachine.Run();
            return new CommandLine(parsedArgs);
        }
    }
}