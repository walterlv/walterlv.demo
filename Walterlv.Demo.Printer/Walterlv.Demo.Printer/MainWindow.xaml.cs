using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace Walterlv.Demo.Printer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XpsDocumentWriter _xpsDocumentWriter;
        private VisualsToXpsDocument _vToXspd;

        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += MainWindow_ContentRendered;
        }

        private async void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            await Task.Delay(1000);

            var pageSize = new Size(1280 * 8, 720 * 8);
            var document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;

            var fixedPage = new FixedPage
            {
                Width = pageSize.Width,
                Height = pageSize.Height,
            };
            for (int i = 0; i < 100; i++)
            {
                fixedPage.Children.Add(new Border
                {
                    Width = pageSize.Width,
                    Height = pageSize.Height,
                    Background = new VisualBrush
                    {
                        Visual = DemoImage,
                    }
                });
            }
            fixedPage.Measure(pageSize);
            fixedPage.Arrange(new Rect(new Point(), pageSize));
            fixedPage.UpdateLayout();

            // Add page to document
            var pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);
            document.Pages.Add(pageContent);

            // Send to the printer.
            var pd = new PrintDialog();
            pd.PrintDocument(document.DocumentPaginator, "正在打印……");

            return;

            using var _localPrintServer = new LocalPrintServer();
            var _currentPrintQueue = _localPrintServer.DefaultPrintQueue;
            _xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(_currentPrintQueue);
            //_xpsDocumentWriter.WritingProgressChanged += XpsDocumentWriter_WritingProgressChanged;
            //_xpsDocumentWriter.WritingPrintTicketRequired += XpsDocumentWriterOnWritingPrintTicketRequired;
            //_xpsDocumentWriter.WritingCancelled += XpsDocumentWriterOnWritingCancelled;
            //_xpsDocumentWriter.WritingCompleted += XpsDocumentWriterOnWritingCompleted;
            _vToXspd = (VisualsToXpsDocument)_xpsDocumentWriter.CreateVisualsCollator();
            _vToXspd.BeginBatchWrite();
            for (int i = 0; i < 100; i++)
            {
                _vToXspd?.WriteAsync(RootPanel);
            }
            _vToXspd?.EndBatchWrite();

        }
    }
}
