using System;
using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Demo.TreePerformance
{
    public sealed class WeightCountLimitCache : IObjectCache
    {
        /// <summary>
        /// 获取或设置能够进入缓存集合中的最大元素个数，必须是正数。
        /// 默认为 <see cref="Int32.MaxValue"/>，即永不过期。
        /// </summary>
        public int CountLimit
        {
            get => _countLimit;
            set => _countLimit = value > 0
                ? value
                : throw new ArgumentOutOfRangeException(nameof(value), @"缓存中限制的最大个数必须是正数。");
        }

        public void Use(object @object)
        {
            if (_hitCountDictionary.TryGetValue(@object, out var weight))
            {
                _hitCountDictionary[@object] = weight + 1;
            }
            else
            {
                _hitCountDictionary[@object] = 1;
            }
        }

        public void Unuse(object @object)
        {
            if (_hitCountDictionary.TryGetValue(@object, out var weight))
            {
                _hitCountDictionary[@object] = weight + 1;
            }
            else
            {
                _hitCountDictionary[@object] = 1;
            }

            var removeCount = _hitCountDictionary.Count - CountLimit;
            for (var i = 0; i < removeCount; i++)
            {

            }
        }

        private void OnOutOfDated(ObjectEventArgs e)
        {
            OutOfDated?.Invoke(this, e);
        }

        public event EventHandler<ObjectEventArgs> OutOfDated;
        private readonly Dictionary<object, int> _hitCountDictionary = new Dictionary<object, int>();
        private int _countLimit = int.MaxValue;
    }
}
