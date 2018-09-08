using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class LongTimeView : UserControl
    {
        public LongTimeView()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Thread.Sleep(500);
            return base.MeasureOverride(constraint);
        }

        private void DelayButton_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(3000);
        }
    }
}
