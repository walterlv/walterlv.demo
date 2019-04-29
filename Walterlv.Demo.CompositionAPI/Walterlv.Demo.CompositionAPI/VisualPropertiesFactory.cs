using Windows.ApplicationModel.Core;

namespace Walterlv.Demo.CompositionAPI
{
    public sealed class VisualPropertiesFactory : IFrameworkViewSource
    {
        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return new VisualProperties();
        }

        private static int Main(string[] args)
        {
            CoreApplication.Run(new VisualPropertiesFactory());

            return 0;
        }
    }
}
