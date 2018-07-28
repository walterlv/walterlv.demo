using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;

namespace Walterlv.Demo.ZeroUwp
{
    internal sealed class WalterlvFrameworkView : IFrameworkView
    {
        /// <summary>
        /// 当应用启动时将执行此方法。进行必要的初始化。
        /// </summary>
        public void Initialize(CoreApplicationView applicationView)
        {
        }

        /// <summary>
        /// 每次应用需要显示一个窗口的时候，此方法就会被调用。用于为当前应用程序显示一个新的窗口视图。
        /// </summary>
        public void SetWindow(CoreWindow window)
        {
            _window = window;
            _window.PointerMoved += OnPointerMoved;

            var compositor = new Compositor();
            _root = compositor.CreateContainerVisual();
            var compositionTarget = compositor.CreateTargetForCurrentView();
            compositionTarget.Root = _root;

            var child = compositor.CreateSpriteVisual();
            child.Size = new Vector2(100f, 100f);
            child.Brush = compositor.CreateColorBrush(Color.FromArgb(0xFF, 0x00, 0x80, 0xFF));
            _root.Children.InsertAtTop(child);
        }

        /// <summary>
        /// 当指针设备在窗口内划过时执行。此时我们来做交互。
        /// </summary>
        private void OnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            var visual = _root.Children.First();
            var position = args.CurrentPoint.Position;
            visual.Offset = new Vector3((float) (position.X - 50f), (float) (position.Y - 50f), 0f);
        }

        /// <summary>
        /// 会在 <see cref="Run"/> 方法执行之前执行。如果需要使用外部资源，那么这时需要将其加载或激活。
        /// </summary>
        public void Load(string entryPoint)
        {
        }

        /// <summary>
        /// 当此方法调用时，需要让应用内的视图（View）显示出来。
        /// </summary>
        public void Run()
        {
            _window.Activate();
            _window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
        }

        /// <summary>
        /// 当应用退出时将执行此方法。如果应用启动期间使用到了外部资源，需要在此时进行释放。
        /// </summary>
        public void Uninitialize()
        {
        }

        private CoreWindow _window;
        private ContainerVisual _root;
    }
}
