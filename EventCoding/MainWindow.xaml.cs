using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.EventCoding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            var output = await GenerateCodesAsync();
            this.OutputTextBox.Text = output;
        }

        private Task<string> GenerateCodesAsync(int length = 4, int count = 200)
        {
            return Task.Run(() => GenerateCodes(length, count));
        }

        private string GenerateCodes(int length = 4, int count = 200)
        {
            var random = new Random();
            var max = (int)Math.Pow(16, length);
            var codes = new int[count];

            for (var i = 0; i < 200; i++)
            {
                var number = random.Next(0, max);
                while (codes.Contains(number))
                {
                    number = random.Next(0, max);
                }
                codes[i] = number;
            }

            return string.Join(Environment.NewLine, codes.Select(x => x.ToString("X").PadLeft(4, '0')));
        }
    }
}
