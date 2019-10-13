using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Walterlv.Demo.Windows
{
    /// <summary>
    /// 为应用程序提供多标签视图。
    /// </summary>
    public class ApplicationTabView : Selector
    {
        static ApplicationTabView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ApplicationTabView),
                new FrameworkPropertyMetadata(typeof(ApplicationTabView)));
        }

        /// <summary>
        /// 多标签视图默认使用 <see cref="TabViewItem"/> 来包装 Data。
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride() => new TabViewItem(this);

        /// <summary>
        /// 判断 Data 是否已经被 <see cref="TabViewItem"/> 包装。
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item) => item is TabViewItem;

        /// <summary>
        /// 标识 <see cref="Header"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(ApplicationTabView), new PropertyMetadata(null));

        /// <summary>
        /// 标识 <see cref="Footer"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            "Footer", typeof(object), typeof(ApplicationTabView), new PropertyMetadata(null));

        /// <summary>
        /// 在多标签前面显示的控件。
        /// </summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 在多标签后面显示的控件（如 + 按钮）。
        /// </summary>
        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }
    }
}
