using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Walterlv.Styles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await Task.Delay(2000);

            Resources["Brush.MainButton.Background"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7a, 0xcc));
        }
    }
}
