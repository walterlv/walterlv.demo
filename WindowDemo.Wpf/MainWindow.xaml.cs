using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingView.IsLoading = true;
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                var url = UrlTextBox.Text;
                var WebView = RootPanel.Children.OfType<WebView>().FirstOrDefault();
                if (WebView == null)
                {
                    WebView = new WebView();
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
                    Thread.Sleep(waitMore);
                }
                LoadingView.IsLoading = false;
            }
        }
    }
}
