using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    public class WalterlvAsyncOperationMethodBuilder<TResult>
    {
        private readonly TaskCompletionSource<TResult> _taskCompletionSource = new TaskCompletionSource<TResult>();

        public static WalterlvAsyncOperationMethodBuilder<TResult> Create()
        {
            Console.WriteLine("[AsyncMethodBuilder] Create");
            return new WalterlvAsyncOperationMethodBuilder<TResult>();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("[AsyncMethodBuilder] Start");
            stateMachine.MoveNext();
        }

        /// <summary>Associates the builder with the specified state machine.</summary>
        /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Console.WriteLine("[AsyncMethodBuilder] SetStateMachine");
        }

        /// <summary>Marks the task as successfully completed.</summary>
        /// <param name="result">The result to use to complete the task.</param>
        public void SetResult(TResult result)
        {
            Console.WriteLine("[AsyncMethodBuilder] SetResult");
            _taskCompletionSource.SetResult(result);
        }

        /// <summary>Marks the task as failed and binds the specified exception to the task.</summary>
        /// <param name="exception">The exception to bind to the task.</param>
        public void SetException(Exception exception)
        {
            Console.WriteLine("[AsyncMethodBuilder] SetException");
            _taskCompletionSource.SetException(exception);
        }

        /// <summary>Gets the task for this builder.</summary>
        public WalterlvAsyncOperation<TResult> Task
        {
            get
            {
                Console.WriteLine("[AsyncMethodBuilder] get_Task");
                return new WalterlvAsyncOperation<TResult>();
            }
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("[AsyncMethodBuilder] AwaitOnCompleted");
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("[AsyncMethodBuilder] AwaitUnsafeOnCompleted");
        }
    }
}
