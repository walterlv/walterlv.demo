using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Windows;

namespace Walterlv.Windows.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var processInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = "walterlv.exe",
                UserName = "lvyi",
                Password = ReadPassword(),
                UseShellExecute = false,
                LoadUserProfile = true
            };
            Process.Start(processInfo);
        }
    }
}
