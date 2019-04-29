using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Walterlv.Demo.BindingContext
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            var scope = FindNameScope(sender);
            var obj = scope.FindName("WalterlvWindow");
            Console.WriteLine(obj);
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var scope = FindNameScope(sender);
            var obj = scope?.FindName("WalterlvWindow");
            Console.WriteLine(obj);
        }

        private static INameScope FindNameScope(object sender)
        {
            var menu = (DependencyObject)sender;
            var scope = NameScope.GetNameScope(menu);
            return scope;
        }
    }
}
