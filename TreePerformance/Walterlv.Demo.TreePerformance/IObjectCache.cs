using System;

namespace Walterlv.Demo.TreePerformance
{
    public interface IObjectCache
    {
        void Use(object @object);
        void Unuse(object @object);
        event EventHandler<ObjectEventArgs> OutOfDated;
    }
}
