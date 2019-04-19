using System.Windows;

namespace Walterlv.Demo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow
            {
                Title = "New Walterlv Demo",
            };
            window.Show();

            base.OnStartup(e);
        }
    }
}