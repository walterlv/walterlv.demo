using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Walterlv.Demo.Weak
{
    class Program
    {
        public static void Main()
        {
            var key1 = new Key("Key1");
            var key2 = new Key("Key2");
            var key3 = new Key("Key3");

            var table = new ConditionalWeakTable<Key, WalterlvValue>
            {
                {key1, new WalterlvValue()},
                {key2, new WalterlvValue()},
                {key3, new WalterlvValue()}
            };

            var weak2 = new WeakReference(key2);
            key2 = null;
            key3 = null;

            GC.Collect();

            Console.WriteLine($@"key1 = {key1?.ToString() ?? "null"}
key2 = {key2?.ToString() ?? "null"}, weak2 = {weak2.Target ?? "null"}
key3 = {key3?.ToString() ?? "null"}
Table = {{{string.Join(", ", table.Select(x => $"{x.Key} = {x.Value}"))}}}");
        }
    }

    public class Key
    {
        private readonly string _name;
        public Key(string name) => _name = name;
        public override string ToString() => _name;
    }

    public class WalterlvValue
    {
        public DateTime CreationTime = DateTime.Now;
        public override string ToString() => CreationTime.ToShortTimeString();
    }
}
