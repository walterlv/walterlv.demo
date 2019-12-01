using System;
using System.Threading.Tasks;
using Walterlv.Demo;

namespace Walterlv.AsyncMethods
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("程序开始");
            await DoAsync();
            Console.WriteLine("程序结束");
        }

        private static async WalterlvAsyncOperation<string> DoAsync()
        {
            Console.WriteLine("异步方法开始");

            await Task.Delay(1000);

            Console.WriteLine("异步方法完成");

            return "doubi";
        }
    }
}
