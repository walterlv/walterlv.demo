using System.Windows;
using System.Windows.Controls;

namespace Walterlv.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainViewModel) DataContext;
            foreach (var item in dataContext.Items)
            {
                item.IsSelected = true;
            }
        }
    }
}
