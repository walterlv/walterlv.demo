using System;
using System.ComponentModel;
using System.Windows;

namespace Walterlv.Demo.DesktopDocking
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            DesktopAppBar.SetAppBar(this, AppBarEdge.Right);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AnotherWindow().Show();
        }
    }

    public static class DesignTimeExtensions
    {
        /// <summary>
        /// 判断一个依赖对象是否是设计时的 <see cref="Window"/>。
        /// </summary>
        /// <param name="window">要被判断设计时的 <see cref="Window"/> 对象。</param>
        /// <returns>如果对象是设计时的 <see cref="Window"/>，则返回 true，否则返回 false。</returns>
        public static bool IsDesignTimeWindow(DependencyObject window)
        {
            if (DesignerProperties.GetIsInDesignMode(window))
            {
                string typeName = window.GetType().FullName;
                if (Equals("Microsoft.Expression.WpfPlatform.InstanceBuilders.WindowInstance", typeName) // Visual Studio 2013
                    || Equals("Microsoft.VisualStudio.DesignTools.WpfDesigner.InstanceBuilders.WindowInstance", typeName)) // Visual Studio 2015/2017/2019
                {
                    return true;
                }
            }
            return false;
        }
    }
}
