using Windows.ApplicationModel.Core;

namespace Walterlv.Demo.ZeroUwp
{
    internal sealed class WalterlvViewSource : IFrameworkViewSource
    {
        public IFrameworkView CreateView()
        {
            return new WalterlvFrameworkView();
        }
    }
}
