﻿using System;
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

        protected void TryInvoke<TSender, TArgs>(WeakEvent<TSender, TArgs> weakEvent, TSender sender, TArgs e)
        {
            var anyAlive = weakEvent.Invoke(sender, e);
            if (!anyAlive)
            {
                OnRefereceLost(_eventSource);
            }
        }

        protected abstract void OnRefereceLost(TEventSource source);
    }

    public class WeakEvent<TSender, TArgs>
    {
        private readonly object _locker = new object();
        private readonly List<WeakReference<Delegate>> _originalHandlers = new List<WeakReference<Delegate>>();
        private readonly List<WeakReference<Action<TSender, TArgs>>> _castedHandlers = new List<WeakReference<Action<TSender, TArgs>>>();

        public void Add(Delegate originalHandler, Action<TSender, TArgs> castedHandler)
        {
            lock (_locker)
            {
                _originalHandlers.Add(new WeakReference<Delegate>(originalHandler));
                _castedHandlers.Add(new WeakReference<Action<TSender, TArgs>>(castedHandler));
            }
        }

        public void Remove(Delegate originalHandler)
        {
            lock (_locker)
            {
                var index = _originalHandlers.FindIndex(x => x.TryGetTarget(out var handler) && handler == originalHandler);
                if (index >= 0)
                {
                    _originalHandlers.RemoveAt(index);
                    _castedHandlers.RemoveAt(index);
                }
            }
        }

        public bool Invoke(TSender sender, TArgs e)
        {
            List<Action<TSender, TArgs>> invokingHandlers = null;
            lock (_locker)
            {
                var handlers = _castedHandlers.ConvertAll(x => x.TryGetTarget(out var target) ? target : null);
                var anyHandlerAlive = handlers.Exists(x => x != null);
                if (anyHandlerAlive)
                {
                    invokingHandlers = handlers;
                }
                else
                {
                    invokingHandlers = null;
                    _castedHandlers.Clear();
                }
            }
            if (invokingHandlers != null)
            {
                foreach (var handler in invokingHandlers)
                {
                    var strongHandler = handler;
                    strongHandler(sender, e);
                }
            }
            return invokingHandlers != null;
        }
    }

    public class WeakEvent<TArgs> : WeakEvent<object, TArgs>
    {
    }
}