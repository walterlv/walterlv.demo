using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Walterlv.ThreadSwitchingTasks
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            var foo = new Foo();

        }


    }

    [DebuggerDisplay("{Count,h}")]
    public class Foo
    {
        public int Count = 3;
    }
}