using System;

namespace Walterlv.Framework
{
    public sealed class ValueAttribute : Attribute
    {
        public int Index { get; }

        public ValueAttribute(int index)
        {
            Index = index;
        }
    }
}