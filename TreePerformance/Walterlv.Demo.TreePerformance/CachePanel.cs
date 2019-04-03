using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Walterlv.Demo.TreePerformance
{
    [ContentProperty(nameof(Children))]
    public class CachePanel : FrameworkElement, IAddChild
    {
        private UIElement _currentChild;
        private IObjectCache _childrenCacheStrategy = new NeverCache();

        public CachePanel()
        {
            Children = new LogicElementCollection(OnAdded, OnRemoved);
        }

        private void OnAdded(UIElement child)
        {
            AddLogicalChild(child);
        }

        private void OnRemoved(UIElement child)
        {
            RemoveLogicalChild(child);
            if (Equals(_currentChild, child))
            {
                CurrentChild = null;
            }
        }

        public Collection<UIElement> Children { get; }

        public UIElement CurrentChild
        {
            get => _currentChild;
            set
            {
                if (value != null && !Children.Contains(value))
                {
                    throw new ArgumentException($@"当前显示的元素必须是 {nameof(Children)} 集合中的其中一个元素。", nameof(value));
                }

                // 接受 null 作为当前显示项，表示当前不显示内容。
                if (_currentChild != null)
                {
                    ChildrenCacheStrategy.Unuse(_currentChild);
                }

                _currentChild = value;
                if (value != null)
                {
                    ChildrenCacheStrategy.Use(value);
                    var parent = VisualTreeHelper.GetParent(value);
                    if (!Equals(parent, this))
                    {
                        AddVisualChild(value);
                    }
                }

                InvalidateArrange();
            }
        }

        public IObjectCache ChildrenCacheStrategy
        {
            get => _childrenCacheStrategy;
            set
            {
                if (value is null) throw new ArgumentNullException(nameof(value));
                if (ReferenceEquals(_childrenCacheStrategy, value)) return;

                _childrenCacheStrategy.OutOfDated -= OnChildOutOfDated;
                _childrenCacheStrategy = value;
                value.OutOfDated += OnChildOutOfDated;
            }
        }

        private void OnChildOutOfDated(object sender, ObjectEventArgs e)
        {
            var value = e.Object as UIElement
                        ?? throw new InvalidOperationException($"当实现 {nameof(IObjectCache)} 时，必须传入 Use 过的元素之一。");
            var parent = VisualTreeHelper.GetParent(value);
            if (Equals(parent, this))
            {
                RemoveVisualChild(value);
            }
        }

        protected override int VisualChildrenCount => CurrentChild == null ? 0 : 1;

        protected override Visual GetVisualChild(int index) => CurrentChild;

        protected override IEnumerator LogicalChildren => Children.GetEnumerator();

        protected override Size MeasureOverride(Size availableSize)
        {
            if (CurrentChild != null)
            {
                CurrentChild.Measure(availableSize);
                return CurrentChild.DesiredSize;
            }

            return default(Size);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (CurrentChild != null)
            {
                CurrentChild.Arrange(new Rect(finalSize));
                return CurrentChild.RenderSize;
            }

            return default(Size);
        }

        void IAddChild.AddChild(object value)
        {
            value = value ?? throw new ArgumentNullException(nameof(value));
            var element = value as UIElement ?? throw new ArgumentException(
                              $@"{nameof(CachePanel)} 的 {nameof(Children)} 必须是 {nameof(UIElement)}", nameof(value));
            Children.Add(element);
        }

        void IAddChild.AddText(string text)
        {
            if (text?.Any(t => !char.IsWhiteSpace(t)) == true)
            {
                throw new ArgumentException($@"文本不能作为 {nameof(CachePanel)} 的 {nameof(Children)}", text);
            }
        }
    }
}
