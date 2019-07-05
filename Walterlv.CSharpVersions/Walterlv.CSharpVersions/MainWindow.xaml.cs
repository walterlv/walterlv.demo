using System.Windows;

namespace Walterlv.CSharpVersions
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool Test()
        {
            var a = (1.0, 1.0);
            var b = (1.0, 1.0);
            return a == b;
        }
    }
}
