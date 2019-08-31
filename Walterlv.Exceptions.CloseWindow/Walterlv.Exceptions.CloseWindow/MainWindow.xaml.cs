using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Walterlv.Exceptions.CloseWindow
{
    /// <summary>
    /// Window.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += OnClosing;
            Closed += OnClosed;
        }

        private async void OnClosing(object sender, CancelEventArgs e)
        {
            //await Task.Yield();
            //var point = PointFromScreen(new Point());
            //Debug.WriteLine(point);
        }

        private void OnClosed(object sender, EventArgs e)
        {
        }
    }
}
