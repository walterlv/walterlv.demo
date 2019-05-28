using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: CompilationRelaxations(CompilationRelaxations.NoStringInterning)]

namespace Walterlv.Demo.StringGC
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new ConditionalWeakTable<string, Foo>
            {
                {"walterlv", new Foo("吕毅")},
                {"lindexi", new Foo("林德熙")},
            };
            var time = DateTime.Now.ToString("T");
            table.Add(time, new Foo("时间"));
            time = null;

            Console.WriteLine($"开始个数：{table.Count()}");
            GC.Collect();
            Console.WriteLine($"剩余个数：{table.Count()}");

            Console.ReadLine();
        }
    }

    public class Foo
    {
        public string Value { get; }
        public Foo(string value) => Value = value;
    }
}
