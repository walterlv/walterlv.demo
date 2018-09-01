using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private ILindexi _remoteLindexi;

        private ILindexi RemoteLindexi
        {
            get
            {
                if (_remoteLindexi == null)
                {
                    var lindexi = (RemoteLindexi)Activator.GetObject(
                        typeof(RemoteLindexi),
                        "ipc://lindexi_server/order");
                    _remoteLindexi = new NativeLindexi(lindexi);

                    _remoteLindexi.CaseOrdered += RemoteLindexiOnCaseOrdered;
                }

                return _remoteLindexi;
            }
        }

        private void RemoteLindexiOnCaseOrdered(object sender, EventArgs e)
        {
            LoggerTextBlock.Dispatcher.InvokeAsync(() => { LoggerTextBlock.Text += "菜已收到"; });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var info = new ProcessStartInfo("Walterlv.Demo.CrossProcess.exe")
            {
            };
            Process.Start(info);

            //var tcpClientChannel = new IpcChannel("lindexi_client");
            //ChannelServices.RegisterChannel(tcpClientChannel, false);
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var result = RemoteLindexi.OrderDinner("腐竹排骨");
            LoggerTextBlock.Text += $"{result}{Environment.NewLine}";
        }
    }
}
