using System;

class Demo
{
    [WalterlvHiddenMethod]
    internal static void Foo()
    {
        Console.WriteLine("我就是一个外部方法。");
    }
}
