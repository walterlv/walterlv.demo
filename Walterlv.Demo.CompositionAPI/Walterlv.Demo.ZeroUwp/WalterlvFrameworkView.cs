using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Interactions;
using Windows.UI.Core;
using Windows.UI.Xaml;

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

            _compositor = new Compositor();
            _root = _compositor.CreateContainerVisual();
            var compositionTarget = _compositor.CreateTargetForCurrentView();
            compositionTarget.Root = _root;

            var child = _compositor.CreateSpriteVisual();
            child.Size = new Vector2(100f, 100f);
            child.Brush = _compositor.CreateHostBackdropBrush();
            _root.Children.InsertAtTop(child);

            InteractionTrackerSetup(_compositor, _root);
        }

        /// <summary>
        /// 当指针设备在窗口内按下时执行。此时我们来做交互。
        /// </summary>
        private void InteractionTrackerSetup(Compositor compositor, Visual hitTestRoot)
        {
            // #1 Create InteractionTracker object
            var tracker = InteractionTracker.Create(compositor);

            // #2 Set Min and Max positions
            tracker.MinPosition = new Vector3(-1000f);
            tracker.MaxPosition = new Vector3(1000f);

            // #3 Setup the VisualInteractionSourc
            var source = VisualInteractionSource.Create(hitTestRoot);

            // #4 Set the properties for the VisualInteractionSource
            source.ManipulationRedirectionMode =
                VisualInteractionSourceRedirectionMode.CapableTouchpadOnly;
            source.PositionXSourceMode = InteractionSourceMode.EnabledWithInertia;
            source.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;

            // #5 Add the VisualInteractionSource to InteractionTracker
            tracker.InteractionSources.Add(source);
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
        private Compositor _compositor;
    }
}
