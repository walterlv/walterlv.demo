using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
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
            var tcpServerChannel = new TcpServerChannel(50632);
            return tcpServerChannel;
        }
    }
}
