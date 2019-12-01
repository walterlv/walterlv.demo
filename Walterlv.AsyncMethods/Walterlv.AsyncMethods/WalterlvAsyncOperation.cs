using System;
using System.Runtime.CompilerServices;

namespace Walterlv.Demo
{
    [AsyncMethodBuilder(typeof(WalterlvAsyncOperationMethodBuilder<>))]
    public class WalterlvAsyncOperation<T>
    {
        public WalterlvAsyncAwaiter<T> GetAwaiter()
        {
            Console.WriteLine("[IAwaitable] GetAwaiter");
            return new WalterlvAsyncAwaiter<T>();
        }
    }

    public class WalterlvAsyncAwaiter<T> : ICriticalNotifyCompletion
    {
        private bool _isCompleted;

        public bool IsCompleted
        {
            get
            {
                Console.WriteLine("[Awaiter] get_IsCompleted");
                return _isCompleted;
            }
            private set => _isCompleted = value;
        }

        public T GetResult()
        {
            Console.WriteLine("[Awaiter] GetResult");
            return default;
        }

        public void OnCompleted(Action continuation)
        {
            Console.WriteLine("[Awaiter] OnCompleted");
            IsCompleted = true;
            continuation();
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            Console.WriteLine("[Awaiter] UnsafeOnCompleted");
            IsCompleted = true;
            continuation();
        }
    }
}
