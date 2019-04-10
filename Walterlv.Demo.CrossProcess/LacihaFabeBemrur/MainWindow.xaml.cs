using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
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
using Walterlv.Demo.RemoteObjects;

namespace LacihaFabeBemrur
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

        private Lindexi _lindexi;
        private RemoteEventHandler<Lindexi, EventArgs> _handler;

        private Lindexi Lindexi
        {
            get
            {
                if (_lindexi == null)
                {
                    var lindexi = (Lindexi)Activator.GetObject(
                        typeof(Lindexi),
                        "ipc://lindexi_server/order");
                    _handler = new RemoteEventHandler<Lindexi, EventArgs>(
                        lindexi, nameof(Lindexi.CaseOrdered), RemoteLindexiOnCaseOrdered);

                    _lindexi = lindexi;
                }

                return _lindexi;
            }
        }

        public void RemoteLindexiOnCaseOrdered(object sender, EventArgs e)
        {
            LoggerTextBlock.Dispatcher.InvokeAsync(() =>
            {
                LoggerTextBlock.Text += "菜已收到";
                LoggerTextBlock.Text += Environment.NewLine;
            });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var info = new ProcessStartInfo("Walterlv.Demo.CrossProcess.exe")
            {
            };
            Process.Start(info);

            var tcpClientChannel = new IpcChannel("lindexi_client");
            ChannelServices.RegisterChannel(tcpClientChannel, false);
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var result = Lindexi.OrderDinner("腐竹排骨");
            LoggerTextBlock.Text += $"{result}{Environment.NewLine}";
        }
    }
}
