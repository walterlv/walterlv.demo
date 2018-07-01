using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MS.Win32;
using Walterlv.Demo.Win32;

namespace Walterlv.Demo
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //var image = (Bitmap)Image.FromFile(@"C:\Users\lvyi\Desktop\Material.png");
            //var hBitmap = image.GetHbitmap();
            //using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            //{
            //    while (true)
            //    {
            //        g.DrawImage(image, new Point(200, 200));
            //    }
            //}

            var source = new CancellationTokenSource();
            //var window = new Win32Window();
            //window.Show();


            var app = new App();
            app.InitializeComponent();
            app.Run();

            //var thread = new Thread(() =>
            //{
            //    var app = new App();
            //    app.InitializeComponent();
            //    app.Run();
            //})
            //{
            //    IsBackground = false
            //};
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            ////window.Loop(source.Token);
            //Console.Read();
        }
    }
}
