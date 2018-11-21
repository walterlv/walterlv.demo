using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LisdairjayKejearNisdoderoSejal
{
    public class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Fantastic fantastic = (IFantastic) null;
            Console.WriteLine(fantastic);
            return 0;
        }
    }

    public class Fantastic
    {
        private readonly IFantastic _value;
        private Fantastic(IFantastic value) => _value = value;
        public static implicit operator Fantastic(IFantastic value) => new Fantastic(value);
        public override string ToString() => $"{_value} is fantastic.";
    }

    public class IFantastic
    {
    }
}