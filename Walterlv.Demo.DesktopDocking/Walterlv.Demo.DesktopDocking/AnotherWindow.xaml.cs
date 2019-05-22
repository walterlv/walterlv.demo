using System;
using System.Windows;

namespace Walterlv.Demo.DesktopDocking
{
    public partial class AnotherWindow : Window
    {
        public AnotherWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            DesktopAppBar.SetAppBar(this, AppBarEdge.Top);
        }
    }
}
