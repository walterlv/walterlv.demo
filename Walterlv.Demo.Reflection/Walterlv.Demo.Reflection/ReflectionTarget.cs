using System;

namespace Walterlv.Demo.Reflection
{
    [ReflectionTarget]
    public class ReflectionTarget
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ReflectionTargetAttribute : Attribute
    {
    }
}
