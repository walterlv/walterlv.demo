using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                await Task.Delay(1000);
                VisibleOr.Visibility = Visibility.Collapsed;
                await Task.Delay(1000);
                VisibleOr.Visibility = Visibility.Visible;
            }
        }
    }
}
