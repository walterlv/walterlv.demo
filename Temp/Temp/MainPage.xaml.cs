using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Temp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 500; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 52;
                //textBlock.Foreground = new SolidColorBrush(Colors.Red);
                textBlock.Text = "一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n一二三四\\n";
                Viewbox Vb = new Viewbox();
                Vb.Projection = new PlaneProjection();
                //((PlaneProjection)Vb.Projection).CenterOfRotationX = 0.5;
                //((PlaneProjection)Vb.Projection).CenterOfRotationY = 0.5;
                ((PlaneProjection)Vb.Projection).RotationY = 35;
                //((PlaneProjection)Vb.Projection).RotationZ = 360;
                Vb.Child = textBlock;
                canvas.Children.Add(Vb);
                //Canvas.SetLeft(Vb, 1);
                //Canvas.SetTop(Vb, 1);
            }
        }
    }
}
