using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Walterlv.Demo.Windows
{
    /// <summary>
    /// 用于承载一个标签的容器。
    /// </summary>
    public class TabViewItem : ListBoxItem
    {
        private readonly Selector _owner;

        static TabViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabViewItem),
                new FrameworkPropertyMetadata(typeof(TabViewItem)));
        }

        /// <summary>
        /// 创建一个标签项，用于承载单个标签项的数据（DataTemplate）。
        /// </summary>
        public TabViewItem()
        {
        }

        /// <summary>
        /// 创建一个被 <see cref="ApplicationTabView"/> 管理的标签项，这可以让一部分交互发生在这两种控件之间。
        /// </summary>
        /// <param name="owner">列表控件。</param>
        public TabViewItem(Selector owner) => _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        /// <summary>
        /// 将 DataTemplate 中的布局约束传递到 <see cref="TabViewItem"/>，这样可以被容器布局时直接获取到。
        /// </summary>
        public override async void OnApplyTemplate()
        {
            var root = await FindContentRootAsync();
            if (root is null)
            {
                return;
            }
            SetBinding(VisibilityProperty, new Binding(VisibilityProperty.Name)
            {
                Source = root,
                Mode = BindingMode.OneWay,
            });
            SetRelatedBinding<double, Thickness>(root, WidthProperty, MarginProperty, (width, margin) => width + margin.Left + margin.Right);
            SetRelatedBinding<double, Thickness>(root, MinWidthProperty, MarginProperty, (width, margin) => width + margin.Left + margin.Right);
            SetRelatedBinding<double, Thickness>(root, MaxWidthProperty, MarginProperty, (width, margin) => width + margin.Left + margin.Right);
            if (Parent is Panel panel)
            {
                panel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// 当按下鼠标时选中此标签。
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (_owner is Selector owner)
            {
                owner.SelectedItem = Content;
            }
        }

        /// <summary>
        /// 找到布局 DataTemplate 的容器。
        /// </summary>
        /// <returns>可等待对象。</returns>
        private async Task<FrameworkElement> FindContentRootAsync()
        {
            var contentPresenter = Template.FindName("LayoutContentHost", this) as DependencyObject;
            if (contentPresenter != null)
            {
                await Dispatcher.Yield();
                var root = VisualTreeHelper.GetChild(contentPresenter, 0) as FrameworkElement;
                return root;
            }
            return null;
        }

        /// <summary>
        /// 设置源对象（<paramref name="source"/>）的两个相关属性绑定到目标的一个属性上，并使用 <paramref name="valueConverter"/> 指定的转换器。
        /// 例如：
        ///  - Width 属性是主属性，源和目标绑定在一起；但有一个相关属性 Margin 会影响到目标的 Width，于是它会作为相关属性加入到源的绑定中。
        ///  - 这样，一旦源的 Width 和 Margin 属性任意一个改变，都会导致目标的 Width 属性同步根据转换器刷新到新值。
        /// </summary>
        /// <typeparam name="TMain">主属性类型。源对象和目标对象的这个属性会绑定。</typeparam>
        /// <typeparam name="TRelated">相关属性类型。愿对象还会额外将此相关属性绑定到目标对象的属性上。</typeparam>
        /// <param name="source">源对象。</param>
        /// <param name="mainProperty">主绑定属性。</param>
        /// <param name="relatedProperty">相关绑定属性。</param>
        /// <param name="valueConverter">绑定值转换器。</param>
        /// <returns>绑定对象，包含用于赋值到目标对象主属性上的一个绑定对象。（但实际上此方法也会同时赋值给目标）。</returns>
        private MultiBinding SetRelatedBinding<TMain, TRelated>(object source,
            DependencyProperty mainProperty, DependencyProperty relatedProperty,
            Func<TMain, TRelated, TMain> valueConverter)
        {
            var binding0 = new Binding(mainProperty.Name)
            {
                Source = source,
                Mode = BindingMode.OneWay,
            };
            var binding1 = new Binding(relatedProperty.Name)
            {
                Source = source,
                Mode = BindingMode.OneWay,
            };
            var binding = new MultiBinding
            {
                Converter = new TwoValuesConverter<TMain, TRelated, TMain>(valueConverter),
            };
            binding.Bindings.Add(binding0);
            binding.Bindings.Add(binding1);
            SetBinding(mainProperty, binding);
            return binding;
        }

        /// <summary>
        /// 用于给两个依赖项属性绑定到一个依赖项属性提供值转换器。
        /// </summary>
        /// <typeparam name="TSource0">第一个源属性类型。</typeparam>
        /// <typeparam name="TSource1">第二个源属性类型。</typeparam>
        /// <typeparam name="TTarget">目标属性类型。</typeparam>
        private class TwoValuesConverter<TSource0, TSource1, TTarget> : IMultiValueConverter
        {
            /// <summary>
            /// 获取两个依赖项属性绑定到一个依赖项属性的值转换器。
            /// </summary>
            private readonly Func<TSource0, TSource1, TTarget> _converter;

            /// <summary>
            /// 创建 <see cref="TwoValuesConverter{TSource0, TSource1, TTarget}"/> 的新实例。
            /// </summary>
            /// <param name="converter"></param>
            public TwoValuesConverter(Func<TSource0, TSource1, TTarget> converter)
                => _converter = converter ?? throw new ArgumentNullException(nameof(converter));

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (values.Length == 2 && values[0] is TSource0 source0 && values[1] is TSource1 source1)
                {
                    return _converter(source0, source1);
                }
                return default(TTarget);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
