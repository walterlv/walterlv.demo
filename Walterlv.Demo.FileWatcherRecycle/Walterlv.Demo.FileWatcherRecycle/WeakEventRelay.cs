using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Walterlv.Demo.FileWatcherRecycle
{
    public abstract class WeakEventRelay<TEventSource> where TEventSource : class
    {
        private readonly TEventSource _eventSource;
        private readonly ConcurrentDictionary<string, string> _events = new ConcurrentDictionary<string, string>();

        protected WeakEventRelay(TEventSource eventSource)
        {
            _eventSource = eventSource ?? throw new ArgumentNullException(nameof(eventSource));
        }

        protected void Subscribe(Action<TEventSource> sourceEventAdder, Action relayEventAdder, [CallerMemberName] string eventName = null)
        {
            if (_events.TryAdd(eventName, eventName))
            {
                sourceEventAdder(_eventSource);
            }

            relayEventAdder();
        }

        protected void Unsubscribe(Action<TEventSource> sourceEventRemover, Action relayEventRemover, [CallerMemberName] string eventName = null)
        {
            relayEventRemover();
        }

        protected void TryInvoke(Func<bool> weakEventInvoker)
        {
            var anyAlive = weakEventInvoker();
            if (!anyAlive)
            {
                OnRefereceLost(_eventSource);
            }
        }

        protected abstract void OnRefereceLost(TEventSource source);
    }

    public class WeakEvent<THandler> where THandler : Delegate
    {
        private readonly object _locker = new object();
        private readonly List<WeakReference<THandler>> _handlers = new List<WeakReference<THandler>>();

        internal void Add(THandler value)
        {
            lock (_locker)
            {
                _handlers.Add(new WeakReference<THandler>(value));
            }
        }

        internal void Remove(THandler value)
        {
            lock (_locker)
            {
                var index = _handlers.FindIndex(x => x.TryGetTarget(out var handler) && handler == value);
                _handlers.RemoveAt(index);
            }
        }

        internal bool Invoke<TSender, TArgs>(TSender sender, TArgs e)
        {
            List<THandler> invokingHandlers = null;
            lock (_locker)
            {
                var handlers = _handlers.ConvertAll(x => x.TryGetTarget(out var target) ? target : null);
                var anyHandlerAlive = handlers.Exists(x => x != null);
                if (anyHandlerAlive)
                {
                    invokingHandlers = handlers;
                }
                else
                {
                    invokingHandlers = null;
                    _handlers.Clear();
                }
            }
            if (invokingHandlers != null)
            {
                foreach (var handler in invokingHandlers)
                {
                    var strongHandler = (Action<TSender, TArgs>)(Delegate)handler;
                    strongHandler(sender, e);
                }
            }
            return invokingHandlers != null;
        }
    }
}
