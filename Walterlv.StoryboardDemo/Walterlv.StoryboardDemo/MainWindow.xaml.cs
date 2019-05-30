using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Walterlv.StoryboardDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Thread.Sleep(10000);
            Duration durationX = new TimeSpan(0, 0, 0, 5, 00);
            Duration durationY = new TimeSpan(0, 0, 0, 5, 00);

            DoubleAnimation moveX = new DoubleAnimation();
            moveX.Duration = durationX;
            moveX.To = 200;
            DoubleAnimation moveY = new DoubleAnimation();
            moveY.Duration = durationY;
            moveY.To = 200;

            moveX.EasingFunction = new StepEasingFunction {NormalizedStep = 0.2};
            moveY.EasingFunction = new StepEasingFunction {NormalizedStep = 0.2};

            Storyboard story1 = new Storyboard();
            story1.Children.Add(moveX);
            story1.Children.Add(moveY);
            Storyboard.SetTarget(moveX, imgGhost);
            Storyboard.SetTarget(moveY, imgGhost);
            Storyboard.SetTargetProperty(moveX, new PropertyPath("(Image.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(moveY, new PropertyPath("(Image.RenderTransform).(TranslateTransform.Y)"));

            story1.Begin();
        }
    }

    public class StepEasingFunction : EasingFunctionBase
    {
        public double NormalizedStep { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new StepEasingFunction {NormalizedStep = NormalizedStep};
        }

        protected override double EaseInCore(double normalizedTime)
        {
            var stepIndex = Math.Round(normalizedTime / NormalizedStep);
            return NormalizedStep * stepIndex;
        }
    }
}
