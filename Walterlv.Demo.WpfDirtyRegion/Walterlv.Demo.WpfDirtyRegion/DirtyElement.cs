using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Walterlv.Demo.WpfDirtyRegion
{
    public class DirtyElement : FrameworkElement
    {
        private readonly List<Visual> _children = new List<Visual>();

        public DirtyElement()
        {
            for (var i = 0; i < 1; i++)
            {
                var child = new DrawingVisual();
                _children.Add(child);
                AddVisualChild(child);
            }

            for (var i = 0; i < 1; i++)
            {
                var child = new DrawingVisual();
                _children.Add(child);
                AddVisualChild(child);
            }
        }

        protected override int VisualChildrenCount => _children.Count;
        protected override Visual GetVisualChild(int index) => _children[index];

        public void Draw()
        {
            var i = 0;
            foreach (var child in _children.OfType<DrawingVisual>())
            {
                using (var dc = child.RenderOpen())
                {
                    var rect = i++ < _children.Count / 2 ? new Rect(200, 0, 2, 400) : new Rect(0, 200, 400, 2);
                    var brush = DateTime.Now.Second % 2 == 0 ? Brushes.Teal : Brushes.Brown;
                    dc.DrawRectangle(brush, null, rect);
                }
            }
        }
    }

    public class DirtyVisual : Visual
    {
        private readonly bool _direction;

        public DirtyVisual(bool direction)
        {
            _direction = direction;
        }
    }
}