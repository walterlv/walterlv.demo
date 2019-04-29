using System;

namespace Walterlv.EqualsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new Foo();
            Console.WriteLine(foo == null);
            Console.WriteLine(foo.Equals(null));
            Console.WriteLine(foo is null);
            Console.WriteLine(Equals(foo, null));
            Console.ReadLine();
        }
    }

    public class Foo
    {
        public override bool Equals(object obj)
        {
            return true;
        }

        public static bool operator ==(Foo left, Foo right)
        {
            return true;
        }

        public static bool operator !=(Foo left, Foo right)
        {
            return !(left == right);
        }
    }
}
