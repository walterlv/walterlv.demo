using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Walterlv.Demo
{
    /// <summary>
    /// 获取可拉伸图片作为 <see cref="Brush"/> 的扩展标记。
    /// </summary>
    [MarkupExtensionReturnType(typeof(Brush))]
    public class FillImageExtension : MarkupExtension
    {
        /// <summary>
        /// 获取或设置作为 <see cref="Brush"/> 的图像。
        /// </summary>
        [ConstructorArgument("source"), DefaultValue(null)]
        public ImageSource Source { get; set; }

        /// <summary>
        /// 获取或设置拉伸图片时周围应该保持不变的边距。
        /// </summary>
        public Thickness Edge { get; set; }

        /// <summary>
        /// 获取或设置补充图片透明部分的背景。
        /// </summary>
        public Brush Background { get; set; }

        /// <summary>
        /// 使用扩展标记“Background="{media:FillImage}"”创建 <see cref="Brush"/>。
        /// </summary>
        public FillImageExtension()
        {
        }

        /// <summary>
        /// 使用扩展标记“Background="{media:FillImage Image.png}"”创建 <see cref="Brush"/>。
        /// </summary>
        /// <param name="source"></param>
        public FillImageExtension(ImageSource source)
        {
            Source = source;
        }

        /// <summary>
        /// 在 xaml 解析时调用，获取一个 <see cref="Brush"/>。这是指定图片进行拉伸后的目标 <see cref="Brush"/>。
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service =
                serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            // 如果没有服务，则直接返回。
            if (service == null)
            {
                return null;
            }
            // MarkupExtension 在样式模板中，返回 this 以延迟提供值。
            if (service.TargetObject.ToString().EndsWith("SharedDp"))
            {
                return this;
            }

            var element = service.TargetObject as FrameworkElement;
            if (element == null)
            {
                return this;
            }
            Brush brush = CreateBrush(element);
            return brush;
        }

        /// <summary>
        /// 在目标元素上创建 <see cref="Brush"/>。
        /// </summary>
        /// <param name="target">用于创建 <see cref="Brush"/> 依赖的 <see cref="FrameworkElement"/>。</param>
        /// <returns>目标 <see cref="Brush"/>。</returns>
        private Brush CreateBrush(FrameworkElement target)
        {
            return CreateAttachedBrush(target, Source, Edge, Background);
        }

        /// <summary>
        /// 在目标元素上创建 <see cref="Brush"/>。
        /// </summary>
        /// <param name="target">用于创建 <see cref="Brush"/> 依赖的 <see cref="FrameworkElement"/>。</param>
        /// <param name="source"></param>
        /// <param name="edge"></param>
        /// <param name="background"></param>
        /// <returns>目标 <see cref="Brush"/>。</returns>
        public static Brush CreateAttachedBrush(FrameworkElement target, ImageSource source, Thickness edge, Brush background)
        {
            var container = new GridImage(source, edge)
            {
                Background = background,
            };
            container.SetBinding(FrameworkElement.WidthProperty, new Binding
            {
                Source = target,
                Path = new PropertyPath("ActualWidth")
            });
            container.SetBinding(FrameworkElement.HeightProperty, new Binding
            {
                Source = target,
                Path = new PropertyPath("ActualHeight"),
            });
            return new VisualBrush(container);
        }

        private class GridImage : FrameworkElement
        {
            private readonly ImageSource _source;
            private readonly double _imageWidth;
            private readonly double _imageHeight;
            private readonly Thickness _edge;
            internal Brush Background { get; set; }

            public GridImage(ImageSource source, Thickness edge)
            {
                if (source == null)
                {
                    throw new ArgumentNullException(nameof(source));
                }
                if (edge.Left < 0 || edge.Top < 0 || edge.Right < 0 || edge.Bottom < 0)
                {
                    throw new ArgumentException(@"图像上的边缘必须大于或等于 0。", nameof(Thickness));
                }
                _source = source;
                _imageWidth = source.Width;
                _imageHeight = source.Height;
                _edge = edge;
            }

            protected override void OnRender(DrawingContext dc)
            {
                if (ActualWidth < _edge.Left + _edge.Right || ActualHeight < _edge.Top + _edge.Bottom)
                {
                    return;
                }
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));
                dc.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
                var hImageLengths = GetLengths(_imageWidth, _edge.Left, _edge.Right);
                var vImageLengths = GetLengths(_imageHeight, _edge.Top, _edge.Bottom);
                var hElementLengths = GetLengths(ActualWidth, _edge.Left, _edge.Right);
                var vElementLengths = GetLengths(ActualHeight, _edge.Top, _edge.Bottom);

                dc.PushGuidelineSet(new GuidelineSet(
                    hElementLengths.Take(3).ToArray(),
                    vElementLengths.Take(3).ToArray()));
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        if (hImageLengths[j + 3] == 0) continue;
                        if (vImageLengths[i + 3] == 0) continue;

                        Brush brush = new ImageBrush(_source)
                        {
                            ViewboxUnits = BrushMappingMode.Absolute,
                            Viewbox = new Rect(
                                hImageLengths[j],
                                vImageLengths[i],
                                hImageLengths[j + 3],
                                vImageLengths[i + 3])
                        };
                        brush.Freeze();
                        dc.DrawRectangle(brush, null, new Rect(
                            hElementLengths[j] + Margin.Left,
                            vElementLengths[i] + Margin.Top,
                            hElementLengths[j + 3],
                            vElementLengths[i + 3]));
                    }
                }
                dc.Pop();
            }

            /// <summary>
            /// 拆分一个长度，并生成一个 <see cref="Array.Length"/> 为 6 的数组。该数组的前三位代表位置，后三位代表长度。
            /// </summary>
            /// <param name="total">被拆分的总长度，如 Width。</param>
            /// <param name="head">被拆分的头长度，如 Left。</param>
            /// <param name="tail">被拆分的尾长度，如 Right。</param>
            private static double[] GetLengths(double total, double head, double tail)
            {
                return new[] { 0, head, total - tail, head, total - head - tail, tail };
            }
        }
    }
}
