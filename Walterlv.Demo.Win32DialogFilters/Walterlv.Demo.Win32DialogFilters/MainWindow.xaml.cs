using System.Windows;
using Microsoft.Win32;

namespace Walterlv.Demo.Win32DialogFilters
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "图片 (*.png, *.jpg)|*.png;*.jpg|文本 (*.txt)|*.txt|walterlv 的自定义格式 (*.lvyi)|*.lvyi";
            dialog.ShowDialog(this);
        }
    }
}
