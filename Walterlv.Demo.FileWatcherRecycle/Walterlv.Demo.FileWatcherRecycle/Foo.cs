using System;
using Walterlv.WeakEvents;

namespace Walterlv.Demo.FileWatcherRecycle
{
    public class Foo
    {
        private readonly WeakEvent<EventArgs> _bar = new WeakEvent<EventArgs>();

        public event EventHandler Bar
        {
            add => _bar.Add(value, value.Invoke);
            remove => _bar.Remove(value);
        }

        private void OnBar() => _bar.Invoke(this, EventArgs.Empty);
    }
}
