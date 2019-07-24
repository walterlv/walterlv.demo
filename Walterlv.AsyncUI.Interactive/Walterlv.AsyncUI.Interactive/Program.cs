using System;
using Walterlv.AsyncUI.Interactive;

namespace Walterlv
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Startup += OnStartup;
            app.Run();

        }

        private static void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
        }
    }
}
