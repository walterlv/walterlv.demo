using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using CommandLine;
using Walterlv.Demo.RemoteObjects;

namespace Walterlv.Demo.CrossProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(Run);
        }

        private static void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private static void Run(Options option)
        {
            Console.WriteLine("林德熙点餐系统正在初始化……");

            var thread = new Thread(() =>
            {
                var lindexi = new Lindexi();
                Console.WriteLine("林德熙创建完成");
                RemotingServices.Marshal(lindexi, "order");
                Console.WriteLine("林德熙已远端化");

                _channel = CreatChannel();
                Console.WriteLine("林德熙已进入通道");
                ChannelServices.RegisterChannel(_channel, false);
                Console.WriteLine("林德熙所在通道已注册完成");
            })
            {
                IsBackground = true,
            };
            thread.Start();

            Console.Read();
        }

        private static IChannel _channel;

        private static IChannel CreatChannel()
        {
            var serverProvider = new BinaryServerFormatterSinkProvider();
            var clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["portName"] = "lindexi_server";

            var channel = new IpcChannel(props, clientProvider, serverProvider);
            //var channel = new IpcChannel("lindexi_server");
            //props["port"] = 17134;
            //var channel = new TcpChannel(props, clientProvider, serverProvider);
            return channel;
        }
    }
}
