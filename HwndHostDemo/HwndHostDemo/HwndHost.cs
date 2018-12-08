using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace HwndHostDemo
{
    public sealed class HwndHost : FrameworkElement
    {
        public HwndHost()
        {
            _hostVisual = new HostVisual();
            _targetSource = new VisualTargetPresentationSource(_hostVisual)
            {
                RootVisual = new Border(),
            };
            AddVisualChild(_hostVisual);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var source1 = (HwndSource) HwndSource.FromVisual(this);
            var source2 = (HwndSource) HwndSource.FromVisual(_targetSource.RootVisual);
        }

        private readonly HostVisual _hostVisual;
        private VisualTargetPresentationSource _targetSource;

        #region Tree & Layout

        protected override Visual GetVisualChild(int index)
        {
            return _hostVisual;
        }

        protected override int VisualChildrenCount => 1;

        #endregion
    }
}
