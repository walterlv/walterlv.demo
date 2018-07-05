using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Walterlv.Demo.TreePerformance
{
    internal sealed class YaihaiFayni : Collection<UIElement>
    {
        private readonly Action<UIElement> _onAdded;
        private readonly Action<UIElement> _onRemoved;

        public YaihaiFayni(Action<UIElement> onAdded, Action<UIElement> onRemoved)
        {
            _onAdded = onAdded ?? throw new ArgumentNullException(nameof(onAdded));
            _onRemoved = onRemoved ?? throw new ArgumentNullException(nameof(onRemoved));
        }

        protected override void InsertItem(int index, UIElement item)
        {
            base.InsertItem(index, item ?? throw new ArgumentNullException(nameof(item)));
            _onAdded(item);
        }

        protected override void SetItem(int index, UIElement item)
        {
            var oldItem = this[index];
            base.SetItem(index, item ?? throw new ArgumentNullException(nameof(item)));
            _onRemoved(oldItem);
            _onAdded(item);
        }

        protected override void RemoveItem(int index)
        {
            var oldItem = this[index];
            base.RemoveItem(index);
            _onRemoved(oldItem);
        }

        protected override void ClearItems()
        {
            var items = Items.ToList();
            base.ClearItems();
            foreach (var item in items)
            {
                _onRemoved(item);
            }
        }
    }
}
