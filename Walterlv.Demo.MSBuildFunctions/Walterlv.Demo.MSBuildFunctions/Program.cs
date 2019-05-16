using System;
using System.IO;

namespace Walterlv.Demo.MSBuildFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var environmentVariable = Environment.GetEnvironmentVariable("TEMP");
            var newTempFolder = @"C:\Walterlv\ApplicationTemp";
            Environment.SetEnvironmentVariable("TEMP", newTempFolder);
            Environment.SetEnvironmentVariable("TMP", newTempFolder);
            var environmentVariable2 = Environment.GetEnvironmentVariable("TEMP");
            var tempPath = Path.GetTempFileName();
            Console.WriteLine("Hello World!");
        }
    }
}
