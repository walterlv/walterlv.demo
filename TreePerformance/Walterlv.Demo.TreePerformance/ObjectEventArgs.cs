using System;

namespace Walterlv.Demo.TreePerformance
{
    public sealed class ObjectEventArgs
    {
        public ObjectEventArgs(object o)
        {
            Object = o ?? throw new ArgumentNullException(nameof(o));
        }

        public object Object { get; }
    }
}
