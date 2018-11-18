using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Walterlv.Demo.Reflection
{
    public class Reflections
    {
        private static readonly Type _targetType = typeof(ReflectionTarget);
        private static Func<ReflectionTarget> _cachedExpressionFunc;

        private static Func<ReflectionTarget> CachedExpressionFunc
        {
            get
            {
                if (_cachedExpressionFunc == null)
                {
                    var @new = Expression.New(typeof(ReflectionTarget));
                    var lambda = Expression.Lambda<Func<ReflectionTarget>>(@new).Compile();
                    _cachedExpressionFunc = lambda;
                }

                return _cachedExpressionFunc;
            }
        }

        [Benchmark]
        public void Assembly()
        {
            var assembly = _targetType.Assembly;
        }

        [Benchmark]
        public void Attributes()
        {
            var attributes = _targetType.Attributes;
        }

        [Benchmark]
        public void CustomAttributes()
        {
            var attribute = _targetType.CustomAttributes.FirstOrDefault(
                x => x.AttributeType == typeof(ReflectionTargetAttribute));
        }

        [Benchmark]
        public void GetCustomAttributesData()
        {
            var attribute = _targetType.GetCustomAttributesData().FirstOrDefault(
                x => x.AttributeType == typeof(ReflectionTargetAttribute));
        }

        [Benchmark]
        public void GetCustomAttributes()
        {
            var attribute = _targetType.GetCustomAttributes(typeof(ReflectionTargetAttribute), false).FirstOrDefault();
        }

        [Benchmark]
        public void GetCustomAttribute()
        {
            var attribute = _targetType.GetCustomAttribute(typeof(ReflectionTargetAttribute), false);
        }

        [Benchmark]
        public void GetCustomAttribute_Generic()
        {
            var attribute = _targetType.GetCustomAttribute<ReflectionTargetAttribute>(false);
        }

        [Benchmark]
        public void GetCustomAttributes_Generic()
        {
            var attribute = _targetType.GetCustomAttributes<ReflectionTargetAttribute>(false);
        }

        [Benchmark]
        public void New()
        {
            var instance = new ReflectionTarget();
        }

        [Benchmark]
        public void Lambda()
        {
            var instance = new ReflectionTarget();
        }

        [Benchmark]
        public void Activator_CreateInstance()
        {
            var instance = (ReflectionTarget) Activator.CreateInstance(_targetType);
        }

        [Benchmark]
        public void Activator_CreateInstance_Generic()
        {
            var instance = Activator.CreateInstance<ReflectionTarget>();
        }

        [Benchmark]
        public void Expression_New()
        {
            var @new = Expression.New(typeof(ReflectionTarget));
            var lambda = Expression.Lambda<Func<ReflectionTarget>>(@new).Compile();
            var instance = lambda.Invoke();
        }

        [Benchmark]
        public void CachedExpression_New()
        {
            var instance = CachedExpressionFunc.Invoke();
        }
    }
}
