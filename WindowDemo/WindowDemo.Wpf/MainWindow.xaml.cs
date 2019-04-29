using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.WPF;

namespace Walterlv.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingView.IsLoading = true;
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                var url = UrlTextBox.Text;
                var WebView = RootPanel.Children.OfType<WebBrowser>().FirstOrDefault();
                if (WebView == null)
                {
                    WebView = new WebBrowser();
                    Grid.SetRow(WebView, 1);
                    RootPanel.Children.Add(WebView);
                }
                WebView.Navigate(url);
            }
            finally
            {
                watch.Stop();
                var waitMore = TimeSpan.FromSeconds(4) - watch.Elapsed;
                if (waitMore > TimeSpan.Zero)
                {
                    await Task.Delay(waitMore);
                    //Thread.Sleep(waitMore);
                }
                LoadingView.IsLoading = false;
            }
        }
    }
}
