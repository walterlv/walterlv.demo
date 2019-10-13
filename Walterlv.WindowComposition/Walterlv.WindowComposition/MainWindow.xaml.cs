using System.Windows;
using System.Windows.Media;
using Walterlv.Windows.Effects;

namespace Walterlv.WindowComposition
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var compositor = new WindowAccentCompositor(this);
            compositor.Composite(Color.FromArgb(0xa0, 0x18, 0xa0, 0x5e));
        }
    }
}
