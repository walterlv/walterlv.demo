using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Walterlv.Demo.Wpf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Walterlv.Demo";

            var dataObject = new DataObject();
            Clipboard.SetDataObject(dataObject);

            Console.Read();
        }
    }
}
