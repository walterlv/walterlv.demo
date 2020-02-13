using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace ImageMetadata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                var header = Metafile.FromFile(@"D:\WIP\Desktop\13d8eebaae19e6ca8d41dff7b9030c16.jpg");
                var witdh = header.Width;
                var height = header.Height;
            }
            watch.Stop();
            var ellapsed = watch.Elapsed;

            var watch2 = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                var bitmap = new Bitmap(@"D:\WIP\Desktop\13d8eebaae19e6ca8d41dff7b9030c16.jpg");
                var witdh = bitmap.Width;
                var height = bitmap.Height;
            }
            watch2.Stop();
            var ellapsed2 = watch2.Elapsed;

            var watch3 = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                var bitmap = new BitmapImage(new Uri(@"D:\WIP\Desktop\13d8eebaae19e6ca8d41dff7b9030c16.jpg", UriKind.Absolute));
                var witdh = bitmap.Width;
                var height = bitmap.Height;
            }
            watch3.Stop();
            var ellapsed3 = watch3.Elapsed;

            var watch4 = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
var decoder = new JpegBitmapDecoder(new Uri(@"D:\WIP\Desktop\13d8eebaae19e6ca8d41dff7b9030c16.jpg", UriKind.Absolute), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
var frame = decoder.Frames[0];
var witdh = frame.PixelWidth;
var height = frame.PixelHeight;
            }
            watch4.Stop();
            var ellapsed4 = watch4.Elapsed;
        }
    }
}
