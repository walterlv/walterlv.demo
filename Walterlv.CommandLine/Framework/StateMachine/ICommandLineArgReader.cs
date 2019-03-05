namespace Walterlv.Framework.StateMachine
{
    internal interface ICommandLineArgReader
    {
        bool Match(string arg);
        void Read(ICommandLineStateMachine reader, string arg);
    }
}