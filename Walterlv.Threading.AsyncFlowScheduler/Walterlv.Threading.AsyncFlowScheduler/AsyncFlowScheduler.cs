using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Threading
{
    public abstract class AsyncFlowScheduler<T>
    {
        protected Func<Task<T>> AsyncAction = async () =>
        {
            await Task.Delay(1000);
            return default;
        };

        public Task<T> RunAsync()
        {
            AsyncAction
        }
    }

    public sealed class ConcurrentAsyncFlowScheduler<T> : AsyncFlowScheduler<T>
    {

    }

    public sealed class FirstAndLastAsyncFlowScheduler<T> : AsyncFlowScheduler<T>
    {

    }

    public sealed class AnyCompletedAsyncFlowScheduler<T> : AsyncFlowScheduler<T>
    {
        
    }

    public sealed class QueueAsyncFlowScheduler<T> : AsyncFlowScheduler<T>
    {

    }

    public sealed class DisableWhenRunningFlowScheduler<T> : AsyncFlowScheduler<T>
    {

    }
}
