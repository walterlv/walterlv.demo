using CommandLine;

namespace Walterlv.Demo.CrossProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(Run);
        }

        private static void Run(Options option)
        {

        }
    }
}
