using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Walterlv.Demo.RoutedEvents
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

        private IInputElement _capturing;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _capturing = (IInputElement) e.OriginalSource;
            _capturing.CaptureMouse();
            Console.WriteLine($"[Down] {e.Source.GetType().Name} | {e.OriginalSource.GetType().Name}");
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"[Move] {e.Source.GetType().Name} | {e.OriginalSource.GetType().Name}");
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine($"[ Up ] {e.Source.GetType().Name} | {e.OriginalSource.GetType().Name}");
            _capturing?.ReleaseMouseCapture();
        }
    }
}
