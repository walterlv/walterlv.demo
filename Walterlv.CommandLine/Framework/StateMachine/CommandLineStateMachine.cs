using System;
using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Framework.StateMachine
{
    internal class CommandLineStateMachine : ICommandLineStateMachine
    {
        private readonly IReadOnlyList<string> _commandLineArgs;
        private Action<string, IReadOnlyList<string>> _optionCollectedAction;
        private string _currentOption;
        private List<string> _currentValues;

        public CommandLineStateMachine(IReadOnlyList<string> args)
        {
            _commandLineArgs = args ?? throw new ArgumentNullException(nameof(args));
        }

        public Dictionary<string, IReadOnlyList<string>> Run()
        {
            // 准备初始参数。
            ICommandLineStateMachine stateMachine = this;
            var parsedArgs = new Dictionary<string, IReadOnlyList<string>>();
            IReadOnlyList<ICommandLineArgReader> readers = new List<ICommandLineArgReader>
            {
                new OptionReader(),
                new ValueReader(),
            };

            // 清除之前状态机运行的所有状态。
            _currentOption = null;
            _currentValues = new List<string>();
            _optionCollectedAction = OnOptionCollected;

            // 执行状态机。
            foreach (var arg in _commandLineArgs)
            {
                var reader = readers.First(x => x.Match(arg));
                reader.Read(stateMachine, arg);
            }

            stateMachine.Commit();

            // 返回结果。
            return parsedArgs;

            // 当状态机运行触发选项生成时，将此选项更新到集合中。
            void OnOptionCollected(string option, IReadOnlyList<string> values)
            {
                if (values.Count > 0)
                {
                    parsedArgs[option ?? ""] = values;
                }
                else if (!string.IsNullOrEmpty(option))
                {
                    parsedArgs[option] = values;
                }
            }
        }

        void ICommandLineStateMachine.SetOption(string arg)
        {
            _currentOption = arg;
            _currentValues = new List<string>();
        }

        void ICommandLineStateMachine.AppendValue(string arg)
        {
            _currentValues.Add(arg);
        }

        void ICommandLineStateMachine.Commit()
        {
            _optionCollectedAction(_currentOption, _currentValues);
        }
    }
}