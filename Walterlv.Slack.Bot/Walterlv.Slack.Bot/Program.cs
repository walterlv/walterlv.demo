using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Walterlv.NullableDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "walterlv's demo";

            int? value = GetValue(1);
            object o = value;

            Console.WriteLine($"value is nullable? {IsOfNullableType(value)}");
            Console.WriteLine($"o     is nullable? {IsOfNullableType(o)}");

            Console.ReadLine();
        }

        private static int? GetValue(int? source) => source;

        static bool IsOfNullableType<T>(T _) => Nullable.GetUnderlyingType(typeof(T)) != null;
    }
}
