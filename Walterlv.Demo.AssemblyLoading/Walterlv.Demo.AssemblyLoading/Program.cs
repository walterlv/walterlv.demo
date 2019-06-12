using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Walterlv.Demo.AssemblyLoading
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await LoadDependencyAssembliesAsync();
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

        private static async Task LoadDependencyAssembliesAsync()
        {
            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dependencies");
            Assembly.LoadFile(Path.Combine(folder, "Ben.Demystifier.dll"));
            Assembly.LoadFile(Path.Combine(folder, "System.Collections.Immutable.dll"));
            Assembly.LoadFile(Path.Combine(folder, "System.Reflection.Metadata.dll"));
        }
    }
}
