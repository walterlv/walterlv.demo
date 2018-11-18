using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Walterlv.Demo.Reflection
{
    public class IsDefinedVsGetCustomAttribute
    {
        private static readonly Type _targetType = typeof(ReflectionTarget);

        [Benchmark(Baseline = true)]
        public void IsDefined()
        {
            var isDefined = _targetType.IsDefined(typeof(ReflectionTargetAttribute), false);
        }

        [Benchmark]
        public void GetCustomAttribute()
        {
            var attribute = _targetType.GetCustomAttribute(typeof(ReflectionTargetAttribute), false);
        }

        [Benchmark]
        public void GetGenericCustomAttribute()
        {
            var attribute = _targetType.GetCustomAttribute<ReflectionTargetAttribute>(false);
        }
    }
}
