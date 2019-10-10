using System;

namespace Walterlv.WindowDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            var windows = WindowEnumerator.FindAll();
            for (int i = 0; i < windows.Count; i++)
            {
                var window = windows[i];
                Console.WriteLine($@"{i.ToString().PadLeft(3, ' ')}. {window.Title}
     {window.Bounds.X}, {window.Bounds.Y}, {window.Bounds.Width}, {window.Bounds.Height}");
            }
            Console.ReadLine();
        }
    }
}
