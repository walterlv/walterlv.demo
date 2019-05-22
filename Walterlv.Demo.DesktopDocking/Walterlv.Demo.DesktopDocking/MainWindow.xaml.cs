using System;
using System.Windows;

namespace Walterlv.Demo.DesktopDocking
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DesktopAppBar.SetAppBar(this, AppBarEdge.Right);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            DesktopAppBar.SetAppBar(this, AppBarEdge.Right);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AnotherWindow().Show();
        }
    }
}
