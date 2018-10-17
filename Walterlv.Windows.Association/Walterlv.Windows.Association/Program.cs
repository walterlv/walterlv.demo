using System;

namespace Walterlv.Windows.Association
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello {string.Join(" ", args)}!");

            var fileAssociation = new FileAssociation("Walterlv.Foo.1");
            fileAssociation.Create();

            Console.ReadLine();
        }
    }
}
