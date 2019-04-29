namespace Walterlv.DirectXDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new D3DRenderer();
            renderer.InitializeDeviceResources();
            renderer.Run();
        }
    }
}
