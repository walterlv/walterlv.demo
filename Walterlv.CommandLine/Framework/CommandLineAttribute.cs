using System;

namespace Walterlv.Framework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class CommandLineAttribute : Attribute
    {
    }
}
