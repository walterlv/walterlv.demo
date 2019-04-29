using System;

namespace Walterlv.Demo.Patterns
{
    public readonly struct FantasticNull<T> where T : class
    {
        public static explicit operator FantasticNull<T>(T value)
        {
            if (value != null) throw new ArgumentException("仅支持从 null 创建。");
            return new FantasticNull<T>();
        }

        public override string ToString() => "null";
    }
}
