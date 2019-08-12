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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Walterlv.Collections.Threading;

namespace Walterlv.Demo.TranslationInVisualTree
{
    public partial class MainWindow : Window
    {
        private readonly AsyncQueue<string> _queue = new AsyncQueue<string>();
        private readonly TranslateTransform _transform = new TranslateTransform();
        private readonly Border _child = new Border
        {
            Width = 100,
            Height = 100,
            Background = Brushes.BlanchedAlmond,
        };

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            _child.RenderTransform = _transform;

            // 1.
            await GotoNextStepAsync("将试验元素插入到可视化树中");

            TestPanel.Children.Add(_child);

            // 2.
            await GotoNextStepAsync("移动元素的位置到 100 100");

            _transform.X = 100;
            _transform.Y = 100;

            // 3.
            await GotoNextStepAsync("将试验元素移出可视化树");

            TestPanel.Children.Remove(_child);

            // 4.
            await GotoNextStepAsync("将试验元素重新加回到可视化树中");

            TestPanel.Children.Add(_child);

            // 5.
            await GotoNextStepAsync("然后再设置一次位置到 200 200");

            _transform.X = 200;
            _transform.Y = 200;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            // Goto next step.
            _queue.Enqueue("");
        }

        private async Task GotoNextStepAsync(string stepDescription)
        {
            NextStepRun.Text = stepDescription;
            await _queue.DequeueAsync();
            LastStepRun.Text = NextStepRun.Text;
        }
    }
}
