using System.Collections.Generic;

namespace Walterlv.FastKeyValue
{
    public sealed partial class FastKeyValue
    {
        private class ReadingStateMachine
        {
            private Dictionary<ReadingState, IReadingState> _states = new Dictionary<ReadingState, IReadingState>
            {
                {ReadingState.Start, new StartState() },
                {ReadingState.Start, new StartState() },
                {ReadingState.Start, new StartState() },
                {ReadingState.Start, new StartState() },
                {ReadingState.Start, new StartState() },
                {ReadingState.Start, new StartState() },
            };

            private ReadingState _current;

            internal ReadingState Input(char input)
            {

            }
        }

        private abstract class IReadingState
        {
            
        }
    }
}
