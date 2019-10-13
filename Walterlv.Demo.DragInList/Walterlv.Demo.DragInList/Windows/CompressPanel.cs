using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Walterlv.Demo.Windows
{
    /// <summary>
    /// 可压缩子元素空间的布局容器。
    /// </summary>
    public class CompressPanel : Panel
    {
        /// <summary>
        /// 在布局开始时，需初始化 <see cref="GridLayout"/> 的一个实例，然后生成测量值 <see cref="_layoutResult"/>。
        /// </summary>
        private GridLayout _layout;

        /// <summary>
        /// 使用此测量值可以进行元素布局。
        /// </summary>
        private GridLayout.MeasureResult _layoutResult;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (InternalChildren.Count == 0)
            {
                return default;
            }

            _layout = new GridLayout(InternalChildren.OfType<FrameworkElement>()
                .Select(x => GetWidthConvention(x)));
            _layoutResult = _layout.Measure(availableSize.Width);

            for (var i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i];
                var width = _layoutResult.LengthList[i];
                child.Measure(new Size(width, availableSize.Height));
            }

            var desiredWidth = _layoutResult.LengthList.Sum();
            var desiredHeight = InternalChildren.OfType<UIElement>().Max(x => x.DesiredSize.Height);
            return new Size(desiredWidth, desiredHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (InternalChildren.Count == 0)
            {
                return default;
            }

            var result = _layout.Arrange(finalSize.Width, _layoutResult);

            var x = 0.0;
            for (var i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i];
                var width = result.LengthList[i];
                child.Arrange(new Rect(x, 0, width, finalSize.Height));
                x += width;
            }

            return finalSize;
        }

        /// <summary>
        /// 根据子元素生成布局宽度约束。
        /// </summary>
        /// <param name="child">子元素。</param>
        /// <returns>宽度约束，布局将依赖于此宽度约束。</returns>
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridLayout.LengthConvention GetWidthConvention(UIElement child)
        {
            if (IsItemsHost)
            {
                // 如果是列表项布局，那么使用 DateTemplate 中的尺寸约束。
                child = Decendents<ContentPresenter>(child)
                    .Select(x => Decendents<FrameworkElement>(x).Skip(1).FirstOrDefault())
                    .FirstOrDefault() ?? child;
            }

            if (child is FrameworkElement fe)
            {
                var isLayoutVisible = child.Visibility != Visibility.Collapsed;
                var width = isLayoutVisible ? fe.Width : 0;
                var minWidth = isLayoutVisible ? fe.MinWidth : 0;
                var maxWidth = isLayoutVisible ? fe.MaxWidth : 0;
                var margin = isLayoutVisible ? fe.Margin.Left + fe.Margin.Right : 0;

                if (double.IsNaN(width) && MinWidth.Equals(0) && double.IsPositiveInfinity(maxWidth))
                {
                    // 如果目标对象没有指定任何约束，那么就按照普通 DesiredSize 布局。
                    width = fe.DesiredSize.Width;
                }

                var widthLength = double.IsNaN(width)
                    ? new GridLength(1, GridUnitType.Star)
                    : new GridLength(width + margin, GridUnitType.Pixel);
                return new GridLayout.LengthConvention(widthLength, minWidth + margin, maxWidth + margin);
            }
            else
            {
                return new GridLayout.LengthConvention();
            }
        }

        /// <summary>
        /// 深度遍历可视化树。
        /// </summary>
        /// <typeparam name="T">要找的可视化对象的类型。</typeparam>
        /// <param name="visual">从此元素开始查找。</param>
        /// <returns>找到的第一个 <typeparamref name="T"/> 类型的可视化对象。</returns>
        [Pure]
        private IEnumerable<T> Decendents<T>(DependencyObject visual)
        {
            if (visual is T t)
            {
                yield return t;
            }
            var count = VisualTreeHelper.GetChildrenCount(visual);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i);
                if (child is Visual c)
                {
                    foreach (var grand in Decendents<T>(c))
                    {
                        yield return grand;
                    }
                }
            }
        }
    }
}
