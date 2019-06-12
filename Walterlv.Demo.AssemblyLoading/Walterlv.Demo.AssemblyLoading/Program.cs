using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Walterlv.Demo.AssemblyLoading
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunAsync();
            Console.ReadLine();
        }

        private static async Task RunAsync()
        {
            try
            {
                await ThrowAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Demystify());
            }

            async Task ThrowAsync() => throw new InvalidOperationException();
        }
    }
}
