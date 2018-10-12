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

namespace Walterlv.Demo.BindingContext
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (var i = 0; i < 5; i++)
            {
                DemoList.Add($"通常的文字 {i}");
            }
        }

        public string DemoText { get; set; } = "只有我才是绑定的文本";

        public List<string> DemoList { get; set; } = new List<string>();
    }
}