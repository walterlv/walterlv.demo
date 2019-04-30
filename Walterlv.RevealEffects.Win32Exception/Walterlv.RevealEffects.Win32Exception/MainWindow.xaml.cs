using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Walterlv.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += OnRendering; 
        }

        private void OnRendering(object sender, EventArgs e)
        {
            DebugTextBlock.Text = Mouse.GetPosition(DebugTextBlock).ToString();
            DebugButton.Content = Mouse.GetPosition(DebugButton).ToString();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
