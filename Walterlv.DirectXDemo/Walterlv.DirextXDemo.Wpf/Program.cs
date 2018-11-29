using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.DirextXDemo.Wpf
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Walterlv File Exists Demo";
            
            var fileInfo = new FileInfo(@":?""");
            var path = Path.GetFullPath(@":?""");
        }
    }
}