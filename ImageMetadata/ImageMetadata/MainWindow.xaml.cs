using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            for (int i = 0; i < 1000; i++)
            {
                File.Copy(@"D:\WIP\Desktop\2020-02-14-large-background-image.jpg",
                    @$"D:\WIP\Desktop\tests\2020-02-14-large-background-image-{i}.jpg", true);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var header = Metafile.FromFile(@$"D:\WIP\Desktop\tests\2020-02-14-large-background-image-{i}.jpg");
                var witdh = header.Width;
                var height = header.Height;
            }
            watch.Stop();
            var ellapsed = watch.Elapsed;

            var watch2 = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var bitmap = new Bitmap(@$"D:\WIP\Desktop\tests\2020-02-14-large-background-image-{i}.jpg");
                var witdh = bitmap.Width;
                var height = bitmap.Height;
            }
            watch2.Stop();
            var ellapsed2 = watch2.Elapsed;

            var watch3 = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var bitmap = new BitmapImage(new Uri(@$"D:\WIP\Desktop\tests\2020-02-14-large-background-image-{i}.jpg", UriKind.Absolute));
                var witdh = bitmap.Width;
                var height = bitmap.Height;
            }
            watch3.Stop();
            var ellapsed3 = watch3.Elapsed;

            var watch4 = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var decoder = new JpegBitmapDecoder(new Uri(@$"D:\WIP\Desktop\tests\2020-02-14-large-background-image-{i}.jpg", UriKind.Absolute), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
                var frame = decoder.Frames[0];
                var witdh = frame.PixelWidth;
                var height = frame.PixelHeight;
            }
            watch4.Stop();
            var ellapsed4 = watch4.Elapsed;
        }
    }
}
