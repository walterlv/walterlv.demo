using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Walterlv.Demo.TouchSimulation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StylusDown += OnStylusDown;
            TouchDown += OnTouchDown;
            MouseDown += OnMouseDown;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var device = new FakeTouchDevice(1, this);
            while (true)
            {
                await Task.Delay(1000);
                device.Down();
            }
        }

        private void OnTouchDown(object sender, TouchEventArgs e)
        {
            Console.WriteLine(e.TouchDevice);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(e.StylusDevice);
        }

        private void OnStylusDown(object sender, StylusDownEventArgs e)
        {
            Console.WriteLine(e.StylusDevice);
        }
    }
}
