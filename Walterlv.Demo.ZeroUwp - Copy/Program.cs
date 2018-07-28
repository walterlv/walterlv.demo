using Windows.ApplicationModel.Core;

namespace Walterlv.Demo.ZeroUwp
{
    public sealed class Program : IFrameworkViewSource
    {
        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return new VisualProperties();
        }

        private static int Main(string[] args)
        {
            CoreApplication.Run(new Program());

            return 0;
        }
    }
}
