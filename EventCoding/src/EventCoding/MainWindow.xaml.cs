using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.EventCoding
{
    using System.Runtime.InteropServices;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            var output = await GenerateCodesAsync();
            OutputTextBox.Text = output;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    Clipboard.SetText(OutputTextBox.Text);
                    break;
                }
                catch (COMException)
                {
                }
            }
        }

        private static Task<string> GenerateCodesAsync(int length = 4, int count = 200)
        {
            return Task.Run(() => GenerateCodes(length, count));
        }

        private static string GenerateCodes(bool hasLetter = true, int length = 4, int count = 200)
        {
            var jnvi = hasLetter ? 16 : 10;
            var format = hasLetter ? "X" : "D";

            var random = new Random();
            var max = (int)Math.Pow(jnvi, length);
            var codes = new int[count];

            for (var i = 0; i < count; i++)
            {
                var number = random.Next(0, max);
                while (codes.Contains(number))
                {
                    number = random.Next(0, max);
                }
                codes[i] = number;
            }

            return string.Join(Environment.NewLine, codes.Select(x => x.ToString(format).PadLeft(4, '0')));
        }
    }
}
