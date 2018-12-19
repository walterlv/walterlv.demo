using System;

namespace Walterlv.Demo.Roslyn
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Console.WriteLine(ReferenceEquals(A, A));
            Console.WriteLine(ReferenceEquals(C, C));
            Console.WriteLine(ReferenceEquals(E, E));
            Console.WriteLine(ReferenceEquals(G, G));
            
            Console.ReadKey(true);
        }

        private static string A => $"walterlv is a {B}";
        private static string B => "逗比";
        private static string C => $"walterlv is a {D}";
        private static string D = "逗比";
        private static string E => $"walterlv is a {F}";
        private static readonly string F = "逗比";
        private static string G => $"walterlv is a {H}";
        private const string H = "逗比";
    }
}
