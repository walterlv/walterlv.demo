using System;
using System.Threading.Tasks;

namespace Walterlv.Threading
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scheduler = new ConcurrentAsyncFlowScheduler();
        }
    }
}
