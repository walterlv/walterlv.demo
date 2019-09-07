using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Walterlv.Demo
{
    /// <summary>
    /// 为 <see cref="FillImageExtension"/> 创建转换器。
    /// <remarks>
    /// 原本 <see cref="FillImageExtension"/> 应该可以用在任何可以显示 <see cref="Brush"/> 的地方。
    /// 但是，由于微软不开放我们实现自己的 <see cref="Brush"/> 子类，现有 <see cref="Brush"/> 又做不到根据控件尺寸自定义排列所绘内容。
    /// 所以我们间接地通过 <see cref="MarkupExtension"/> 来实现了好像在用 <see cref="Brush"/> 的功能。
    /// 不过，既然无法自定义排列所绘内容，那么就在 <see cref="MarkupExtension"/> 里通过获取控件尺寸间接排列；
    /// 然而 <see cref="Style"/> 里是获取不到控件的，于是只好通过绑定的方式间接获取控件。
    /// 这个类就是那个和绑定配合使用的转换器。
    /// </remarks>
    /// </summary>
    internal class FillImageBrush : DependencyObject, IValueConverter
    {
        public static readonly FillImageBrush Instance = new FillImageBrush();

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(FillImageBrush), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty EdgeProperty = DependencyProperty.Register(
            "Edge", typeof(Thickness), typeof(FillImageBrush), new PropertyMetadata(default(Thickness)));

        public Thickness Edge
        {
            get { return (Thickness)GetValue(EdgeProperty); }
            set { SetValue(EdgeProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(FillImageBrush), new PropertyMetadata(default(Brush)));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement target = (FrameworkElement)value;
            FillImageBrush local = (FillImageBrush)parameter;
            return FillImageExtension.CreateAttachedBrush(target, local.Source, local.Edge, local.Background);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
