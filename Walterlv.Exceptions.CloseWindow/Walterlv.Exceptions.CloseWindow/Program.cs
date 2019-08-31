using System;
using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.Exceptions.CloseWindow
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var app = new Application
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown,
            };
            app.Startup += App_Startup;
            app.Run();
        }

        private static async void App_Startup(object sender, StartupEventArgs e)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                var window = new MainWindow
                {
                    Left = 3000,
                };
                window.Show();
                await Task.Yield();
                window.WindowState = WindowState.Maximized;
                await Task.Delay(1000);
                window.Close();
            }
        }
    }
}
