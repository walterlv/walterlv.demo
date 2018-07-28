using Windows.ApplicationModel.Core;

namespace Walterlv.Demo.ZeroUwp
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            CoreApplication.Run(new WalterlvViewSource());
        }
    }
}
