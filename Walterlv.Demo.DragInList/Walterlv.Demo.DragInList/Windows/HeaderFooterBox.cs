using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// 表示一个可以显示眉和脚的控件。
    /// </summary>
    public class HeaderFooterBox : Decorator
    {
        /// <summary>
        /// 标识 <see cref="Orientation"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(HeaderFooterBox),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        [ContractPublicPropertyName(nameof(Header))]
        private UIElement _header;

        [ContractPublicPropertyName(nameof(Footer))]
        private UIElement _footer;

        /// <summary>
        /// 表示眉和脚的方向。
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 获取或设置眉。
        /// </summary>
        [DefaultValue(null)]
        public virtual UIElement Header
        {
            get => _header;
            set => SetChild(ref _header, value);
        }

        /// <summary>
        /// 获取或设置脚。
        /// </summary>
        [DefaultValue(null)]
        public virtual UIElement Footer
        {
            get => _footer;
            set => SetChild(ref _footer, value);
        }

        protected override int VisualChildrenCount => Count(Header) + Count(Child) + Count(Footer);

        protected override Visual GetVisualChild(int index) => ChildAt(index, Header, Footer, Child);

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (Header != null)
                {
                    yield return Header;
                }
                if (Footer != null)
                {
                    yield return Footer;
                }
                if (Child != null)
                {
                    yield return Child;
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                {
                    Header?.Measure(availableSize);
                    Footer?.Measure(availableSize);
                    var headerDesiredSize = Header?.DesiredSize ?? default;
                    var footerDesiredSize = Footer?.DesiredSize ?? default;

                    var width = availableSize.Width - headerDesiredSize.Width - footerDesiredSize.Width;
                    width = width < 0 ? 0 : width;
                    Child?.Measure(new Size(width, availableSize.Height));
                    var childDesiredSize = Child?.DesiredSize ?? default;

                    return new Size(
                        headerDesiredSize.Width + footerDesiredSize.Width + childDesiredSize.Width,
                        Math.Max(Math.Max(headerDesiredSize.Height, footerDesiredSize.Height), childDesiredSize.Height));
                }
                case Orientation.Vertical:
                {
                    Header?.Measure(availableSize);
                    Footer?.Measure(availableSize);
                    var headerDesiredSize = Header?.DesiredSize ?? default;
                    var footerDesiredSize = Footer?.DesiredSize ?? default;

                    var height = availableSize.Height - headerDesiredSize.Height - footerDesiredSize.Height;
                    height = height < 0 ? 0 : height;
                    Child?.Measure(new Size(availableSize.Width, height));
                    var childDesiredSize = Child?.DesiredSize ?? default;

                    return new Size(
                        Math.Max(Math.Max(headerDesiredSize.Width, footerDesiredSize.Width), childDesiredSize.Width),
                        headerDesiredSize.Height + footerDesiredSize.Height + childDesiredSize.Height);
                }
                default:
                {
                    return default;
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                {
                    var x = 0.0;

                    if (Header != null)
                    {
                        Header.Arrange(new Rect(x, 0, Header.DesiredSize.Width, finalSize.Height));
                        x += Header.RenderSize.Width;
                    }

                    if (Child != null)
                    {
                        Child.Arrange(new Rect(x, 0, Child.DesiredSize.Width, finalSize.Height));
                        x += Child.RenderSize.Width;
                    }

                    if (Footer != null)
                    {
                        Footer.Arrange(new Rect(x, 0, Footer.DesiredSize.Width, finalSize.Height));
                    }

                    return finalSize;
                }
                case Orientation.Vertical:
                {
                    var y = 0.0;

                    if (Header != null)
                    {
                        Header.Arrange(new Rect(0, y, finalSize.Width, Header.DesiredSize.Height));
                        y = Header.RenderSize.Height;
                    }

                    if (Child != null)
                    {
                        Child.Arrange(new Rect(0, y, finalSize.Width, Child.DesiredSize.Height));
                        y += Child.RenderSize.Height;
                    }

                    if (Footer != null)
                    {
                        Footer.Arrange(new Rect(0, y, finalSize.Width, Footer.DesiredSize.Height));
                    }

                    return finalSize;
                }
                default:
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// 设置子控件并将其加入到可视化树和逻辑树。
        /// </summary>
        /// <param name="field">子控件的字段。</param>
        /// <param name="value">子控件的新值。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetChild(ref UIElement field, UIElement value)
        {
            if (field != value)
            {
                RemoveVisualChild(field);
                RemoveLogicalChild(field);
                field = value;
                AddLogicalChild(value);
                AddVisualChild(value);
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// 计算某个控件是否应该加入到个数计数。
        /// </summary>
        /// <param name="child">要检查个数的元素。</param>
        /// <returns>如果元素不为 null 则计数 1，否则计数 0。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Count(UIElement child) => child is null ? 0 : 1;

        /// <summary>
        /// 在一组元素的集合中确定某个序号处的非空元素。
        /// </summary>
        /// <param name="index">序号。</param>
        /// <param name="children">元素集合。</param>
        /// <returns>序列中非空部分序号处的元素。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UIElement ChildAt(int index, params UIElement[] children)
        {
            var i = 0;
            foreach (var child in children)
            {
                if (child is null)
                {
                    continue;
                }

                if (i == index)
                {
                    return child;
                }

                i++;
            }

            return null;
        }
    }
}
