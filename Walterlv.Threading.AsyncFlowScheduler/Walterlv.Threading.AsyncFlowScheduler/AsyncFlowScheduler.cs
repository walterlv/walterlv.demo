using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Threading
{
    public abstract class AsyncFlowScheduler<T>
    {
        protected Func<Task<T>> 

        public Task<T> RunAsync(CancellationToken cancellationToken = null)
        {
            Task.Run()
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
