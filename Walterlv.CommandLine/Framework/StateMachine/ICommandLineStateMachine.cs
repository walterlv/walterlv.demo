namespace Walterlv.Framework.StateMachine
{
    internal interface ICommandLineStateMachine
    {
        void SetOption(string arg);
        void AppendValue(string arg);
        void Commit();
    }
}