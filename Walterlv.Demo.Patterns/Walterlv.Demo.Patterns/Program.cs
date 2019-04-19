using System;
using Walterlv.ERMail.OAuth;

namespace Walterlv.Demo.Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Scope scope = "A";
            var full = scope | "B" | "C";
            Console.WriteLine(full);
        }
    }

    public class Fantastic
    {
        public static Fantastic operator +(Fantastic a, string b)
        {
            return new Fantastic();
        }
    }
}