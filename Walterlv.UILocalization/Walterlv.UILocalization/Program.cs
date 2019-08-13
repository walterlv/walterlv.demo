using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.UILocalization
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
