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
using System.Windows.Markup;
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

            DataContext = this;
        }

        public string DemoText { get; set; } = "只有我才是绑定的文本";

        public List<DemoItem> DemoList { get; set; } = new List<DemoItem>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var scope = FindNameScope(sender);
            var code = scope.GetHashCode();
            Console.WriteLine(code);
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            var scope = FindNameScope(sender);
            var code = scope.GetHashCode();
            Console.WriteLine(code);
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var scope = FindNameScope(sender);
            var code = scope?.GetHashCode();
            Console.WriteLine(code);
        }

        private static INameScope FindNameScope(object sender)
        {
            var menu = (DependencyObject) sender;
            var scope = NameScope.GetNameScope(menu);
            return scope;
        }
    }

    public class DemoItem
    {
        private readonly string _value;

        private DemoItem(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator DemoItem(string value)
        {
            return new DemoItem(value);
        }
    }

    public sealed class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(default(object)));

        public object Data
        {
            get { return (object) GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public override string ToString()
        {
            if (Data is FrameworkElement fe)
            {
                return $"BindingProxy: {fe.Name}";
            }
            return $"Binding Proxy: {Data?.GetType().FullName}";
        }
    }
}