using System;

namespace Walterlv.Demo.TreePerformance
{
    internal class NeverCache : IObjectCache
    {
        public void Use(object @object)
        {
        }

        public void Unuse(object @object)
        {
        }

        public event EventHandler<ObjectEventArgs> OutOfDated;
    }
}
